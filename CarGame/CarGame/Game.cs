
using System.Data;
using System.Numerics;
using Raylib_cs;
using rlImGui_cs;
using static Raylib_cs.Raylib;

namespace CarGame;

public class Game
{

    private int SCREEN_W;
    private int SCREEN_H;

    private string work_dir;
    
    public Image car_img;
    public Texture2D track;

    public Camera2D camera;
    
    private Car car;

    public bool running;

    public float prev_rotation;
    
    public Game(int screenW, int screenH)
    {
        running = true;
        SCREEN_W = screenW;
        SCREEN_H = screenH;
        prev_rotation = 0f;
        InitWindow(SCREEN_W, SCREEN_H, "Game");
        SetTargetFPS(60);
        
        rlImGui.Setup(true);

        work_dir = "../../../";
        
        car_img = LoadImage(work_dir+"Textures/car.png");
        track = LoadTexture(work_dir + "Textures/track.png");
        
        car = new Car(new Vector2(640, 360), new Vector2(0, 0), 150f, 400f, 60f, 20f, car_img, CarType.Unstable);

        camera = new Camera2D(new Vector2(SCREEN_W/2, SCREEN_H/2), new Vector2(car.rect.X- car.rect.Width, car.rect.Y- car.rect.Height), (float)car.rotation, 1.0f);
    }

    private void Update()
    {

        if (WindowShouldClose())
        {
            running = false;
        }
        
        float dt = GetFrameTime();
        
        car.Update(dt);

        camera.Target = new Vector2(float.Lerp(camera.Target.X, car.rect.X, 0.5f),
            float.Lerp(camera.Target.Y, car.rect.Y, 0.5f));

       /* if (Math.Ceiling(car.rotation) != Math.Ceiling(camera.Rotation))
        {
            camera.Rotation += (float)Math.Ceiling(car.rotation) - prev_rotation;
            prev_rotation = (float)Math.Ceiling(car.rotation);
        }*/
        
       // camera.Rotation = (float)Math.Ceiling(car.rotation) + 90f;
        
    }

    private void Draw()
    {
        BeginDrawing();
        ClearBackground(Color.Green);
       // DrawText("Car rotation: "+ car.rotation, 10, 500, 32, Color.Black);
       // DrawText("Camera rotation: "+ camera.Rotation, 10, 600, 32, Color.Black);
        BeginMode2D(camera);
        DrawTexture(track, 0, 0, Color.White);
        car.Draw(camera);
        EndMode2D();
        car.DrawUI();
        EndDrawing();
    }

    public void Terminate()
    {
        rlImGui.Shutdown();
        CloseWindow();
    }

    public void Run()
    {
        if (running)
        {
            Update();
            Draw();
        }
        else
        {
            Terminate();
        }
        
        
    }

    public void LoadRes()
    {
        
    }


}