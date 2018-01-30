using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game1.Utilities;
using Game1.Entities;
using Game1.Engine;
using Game1.Input;

namespace Game1.Engine
{
    public abstract class BoardAbstract{

        public static int B_WIDTH;// = 400;	
        public static int B_HEIGHT;// = 300;

        private EntityUpdater updatingState = new EntityUpdater();
        private InactiveEntityUpdater pausedState = new InactiveEntityUpdater();
        private EntityUpdater currentState;

        //protected TimerTask updateEntitiesTask;

        private bool isItterating;

        private DoubleLinkedList<EntityStatic> updateableEntitiesList = new DoubleLinkedList<EntityStatic>();

        int counter;

        protected OverlayComposite diagnosticsOverlay;

        //protected ArrayList<EntityStatic> entitiesList; 

        public RenderingEngine renderingEngine;
        protected MovingCamera camera;
        protected List<SlidingMessagePopup> slidingMessageQueue;
        //protected ConcurrentHashMap<Integer, SlidingMessagePopup> slidingMessageQueue;

        private Console console;
	    private ConsoleActive consoleActive = new ConsoleActive();
        private ConsoleDisabled consoleDisabled = new ConsoleDisabled();
        private IConsoleState currentConsoleState = consoleDisabled;

        public CollisionEngine collisionEngine;

        protected Scene currentScene = new Scene(this);

        protected int[] speedLogDraw = new int[300];
        protected int[] speedLog = new int[300];

        protected EditorPanelBoard editorPanel;
        protected JFrame mainFrame;


        public BoardAbstract(int width, int height, JFrame frame)
        {
            currentState = updatingState;

            B_WIDTH = width;
            B_HEIGHT = height;
            mainFrame = frame;

            console = new Console(20, B_HEIGHT - 200, this);
            //slidingMessageQueue = new ConcurrentHashMap<>();
            slidingMessageQueue = new ArrayList<>();
            frame.addMouseWheelListener(this);
            frame.addKeyListener(this);
            /*  editorPanel = new EditorPanel(this);
              editorPanel.setSize(new Dimension(240, 300));
              editorPanel.setPreferredSize(new Dimension(240, 300));*/
        }


        protected void postInitializeBoard()
        {
    
	    }


        protected abstract void initEditorPanel();

        public bool isWorking()
        {
            return isItterating;
        }

        public void setMainFrame(JFrame frame)
        {
            this.mainFrame = frame;
        }

        protected void addInputController(InputManager inputController)
        {

            if (inputController instanceof KeyListener ){
                mainFrame.addKeyListener((KeyListener)inputController);
            }
		    else if (inputController instanceof ControllerListener){    //INPUT CONTROLLER TO BE ADDED IS CONTROLLER INPUT

            }

            testControllerThread.start();
        }

	    protected void activeRenderingDraw()
        {

        }

        protected void paintFrame()
        {

        }

        protected InputManagerMouseKeyboard getUnpausedInputController()
        {
            return this.updatingState.inputController;
        }
        protected InputManagerMouseKeyboard getPausedInputController()
        {
            return this.pausedState.inputController;
        }
        protected InputManagerMouseKeyboard getCurrentBoardInputController()
        {
            return this.currentState.inputController;
        }

        protected void pauseUpdater()
        {
            this.currentState = this.pausedState;
        }
        protected void activateUpdater()
        {
            this.currentState = this.updatingState;
        }
        protected void advanceUpdater()
        {
            //FIXME: Remove this:
            this.updatingState.update();

        }
        protected void advanceUpdater(byte frames)
        {
            for (int i = 0; i < frames; i++)
                this.updatingState.update();
        }

        public void activeRender(Graphics2D g)
        {

            this.currentConsoleState.render(g);
        }

        public static int randomInt(int min, int max)
        {

            Random temporaryRandom = new Random();
            int returnInt = temporaryRandom.nextInt(max - min);
            temporaryRandom = null;
            return min + returnInt;
        }

    interface IConsoleState
    {

        void render(Graphics2D g);
        void keyPressed(KeyEvent e);
    }

    class ConsoleDisabled : IConsoleState
    {

        public void render(Graphics2D g)
        {
            graphicsThreadPaint(g);
        }
        public void keyPressed(KeyEvent e)
        {

        }
	}

	class ConsoleActive : IConsoleState{

        public void render(Graphics2D g)
        {
            graphicsThreadPaint(g);
            console.drawConsole((Graphics2D)g);
        }
        public void keyPressed(KeyEvent e)
        {
            console.inputEvent(e);
        }
	}
	
	protected abstract void graphicsThreadPaint(Graphics2D g);

protected abstract void entityThreadRun();

public EntityStatic[] listCurrentSceneEntities()
{
    return this.currentScene.listEntities();
}

public MovingCamera getCamera() throws NullPointerException
{
		return this.camera;
}
public EditorPanelBoard getBoardEditorPanel()
{
    return this.editorPanel;
}
public JPanel getEditorJPanel()
{
    return this.editorPanel;
}
/*public ConcurrentHashMap<Integer, SlidingMessagePopup> getSlidingMessageQueue() {
    return this.slidingMessageQueue;
}*/
public ArrayList<SlidingMessagePopup> getSlidingMessageQueue()
{
    return this.slidingMessageQueue;
}
public void transferEditorPanel(EditorPanelBoard instance)
{
    this.editorPanel = instance;
}

public Scene getCurrentScene()
{
    return this.currentScene;
}


public void changeScene(Scene scene)
{
    this.renderingEngine.debugClearRenderer();
    this.collisionEngine.degubClearCollidables();
    this.updateableEntitiesList.clear();
    this.currentScene = scene;
}

//ENTITY ADDING AND NOTIFYING

public void addEntityToCurrentScene(EntityStatic entity)
{
    this.currentScene.addEntity(entity, 0);
}

public void notifyGraphicsAddition(GraphicComposite graphicsComposite)
{
    this.renderingEngine.addGraphicsCompositeToRenderer(graphicsComposite);
}

public ListNodeTicket<EntityStatic> addEntityToUpdater(EntityStatic entity)
{
    return updateableEntitiesList.add(entity);
}

protected int updateableEntities()
{
    return updateableEntitiesList.size();
}

protected class BoundaryOverlay : Overlay
{


        protected Font defaultFont = new Font(Font.SANS_SERIF, Font.PLAIN, 12);
protected Font contextFont = new Font(Font.SANS_SERIF, Font.PLAIN, 16);

@Override
        public void paintOverlay(Graphics2D g2, MovingCamera cam)
        {

            Collider[] colliders = collisionEngine.debugListActiveColliders();

            cam.setColor(Color.CYAN);
            g2.setColor(Color.CYAN);
            g2.setFont(contextFont);
            g2.drawString("BOUNDARY OVERLAY", 300, 30);
            g2.setFont(defaultFont);
            g2.drawString("Active Colliders: " + colliders.length, 300, 45);

            for (Collider collider : colliders)
            {
                collider.debugDrawBoundary(cam, g2);
                cam.drawCrossInWorld(collider.getOwnerEntity().getPosition(), g2);
            }

        }
	}
	
	
	protected class DiagnosticsOverlay implements Overlay
{

    @Override

        public void paintOverlay(Graphics2D g2, MovingCamera cam)
{

    g2.setColor(new Color(0, 0, 0, 150));
    g2.fillRect(0, 0, B_WIDTH, B_HEIGHT);
    //DIAGNOSTIC GRAPH
    g2.setColor(Color.CYAN);
    for (int i = 0; i < 320; i += 20)
    {
        g2.drawLine(45, 500 - i, 1280, 500 - i);
        g2.drawString(i / 20 + " ms", 10, 603 - i);
    }

    for (int i = 0; i < speedLog.length; i++)
    {

        int speed = speedLogDraw[i] / 50; //1ms = 20px
        g2.setColor(Color.MAGENTA);
        g2.drawLine(3 * i + 50, 500, 3 * i + 50, 500 - speed);
        g2.setColor(Color.CYAN);
        g2.drawLine(3 * i + 50, 500 - speed, 3 * i + 50, 500 - speed - (speedLog[i] / 50));
    }

    g2.drawString("Updateable entities: " + BoardAbstract.this.updateableEntities(), 400, 20);

}
		
	}
	
	
	class EntityUpdater{

        protected bool isPaused;
        internal InputManagerMouseKeyboard inputController;

        public EntityUpdater(InputManagerMouseKeyboard inputController)
        {
            this.inputController = inputController;
        }

        private class ConsoleToggle : KeyCommand
        {

            private bool consoleIsVisible = false;

            public void onPressed()
            {
                diagnosticsOverlay.toggle();
                if (consoleIsVisible)
                {
                    currentConsoleState = consoleDisabled;
                    consoleIsVisible = false;
                }
                else
                {
                    currentConsoleState = consoleActive;
                    consoleIsVisible = true;
                }
            }
        }

        private class PauseToggle : KeyCommand
        {
            public void onPressed()
            {
                pauseUpdater();
                //setIsPausedAndNotifyThreads(true);
                //test: 
                cancelTimerTasks();
            }
        }

        public EntityUpdater()  //UPDATER CLASS
        {
            isPaused = false;
            inputController = new InputManagerMouseKeyboard("Abstract Board Updating Input Controller");

            inputController.createKeyBinding( 192, new ConsoleToggle() );
            inputController.createKeyBinding( KeyEvent.VK_PAUSE, new PauseToggle );

		}


        public void setIsPausedAndNotifyThreads(bool choice)
        {
            this.isPaused = choice;
            notifyAll();
        }

        public void update()
        {

            isItterating = true;

            while (updateableEntitiesList.hasNext())
            {
                updateableEntitiesList.get().updateComposites();
            }

            entityThreadRun();

            while (updateableEntitiesList.hasNext())
            {
                updateableEntitiesList.get().updateEntity();
            }

            isItterating = false;
        }
		
	}
	
	class InactiveEntityUpdater extends EntityUpdater{


        protected InactiveEntityUpdater()
        {
            super(new InputManagerMouseKeyboard("Board Abstract Pauced Input Controller"));

            inputController.createKeyBinding(KeyEvent.VK_PAUSE, new KeyCommand()
            {

                        public void onPressed()
            {
                activateUpdater();
                initializeTimerTasks();
            }
        });
			inputController.createKeyBinding(KeyEvent.VK_DIVIDE, new KeyCommand()
{
    public void onPressed()
    {
        advanceUpdater();
        //getTheMainTask().run();
    }
});
		}
		
		public void update()
{
    //FIXME STOP TIMER RATHER THAN DOING NOTHING at 60FPS
    System.err.println("BoardAbstract#update(): current thread: " + Thread.currentThread());
    /*while (isPaused) {
        try {
            wait();
        } catch (InterruptedException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
    }*/
}	
		
	}
	

        public void keyPressed(KeyEvent e)
        {
            this.currentState.inputController.keyPressed(e);
            this.currentConsoleState.keyPressed(e);
        }

        public void keyReleased(KeyEvent e)
        {
            this.currentState.inputController.keyReleased(e);
        }

        public void keyTyped(KeyEvent e)
        {

        }


        protected int updateableEntitiesNumber()
        {
            return this.updateableEntitiesList.size();
        }
	
    }
}
