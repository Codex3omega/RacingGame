using System.Drawing;
using System.Numerics;
using ImGuiNET;
using MathNet.Numerics.Distributions;
using Raylib_cs;
using rlImGui_cs;
using static Raylib_cs.Raylib;
using Color = Raylib_cs.Color;
using Rectangle = Raylib_cs.Rectangle;

namespace CarGame;

public enum CarType{
    Normal,
    Unstable,
    Fast,
    Fast_Stable
}

public class Car
{
    private CarType type;
    
    public Vector2 pos;
    public Vector2 velocity;
    public Vector2 size;
    private float aceeleration; // 1
    
    private bool moving;
    private bool drifting;
    
    private float max_velocity;
    private float friction; // 2
    public double rotation;
    private Texture2D texture; 
    private float drift_angle; 
    private float drift_bias; // 3

    private float steering; 
    private float steering_speed; // 4
    private float max_steering; 
    private float steer_friction; // 6
    private double radians;
    
    public Rectangle rect
    {
        get
        {
           return new Rectangle(pos, size.Y, size.X);
        }
    }

    public Car(Vector2 _pos,Vector2 _size, float _speed, float max_vel, float fric, float _drift_bias, Image img, CarType _type)
    {
        pos = _pos;

        radians = 0;
        
       ImageResize(ref img, 93, 43);
       ImageRotate(ref img, -90);


       type = _type;
       if (type == CarType.Normal)
       {
           aceeleration = 200;
           max_velocity = 630;
           friction = 60;
           drift_bias = 20;
           steering_speed  = 0.7f;
           max_steering = 3.5f;
           steer_friction = 0.1f;

       }
       
      else  if (type == CarType.Unstable)
       {
           aceeleration = 200;
           max_velocity = 630;
           friction = 60;
           drift_bias = 40;
           steering_speed  = 0.7f;
           max_steering = 3.5f;
           steer_friction = 0.1f;

       }
      
       else  if (type == CarType.Fast)
      {
          aceeleration = 200;
          max_velocity = 815;
          friction = 60;
          drift_bias = 40;
          steering_speed  = 0.7f;
          max_steering = 3.5f;
          steer_friction = 0.1f;

      }
       else  if (type == CarType.Fast_Stable)
       {
           aceeleration = 325;
           max_velocity = 825;
           friction = 60;
           drift_bias = 10;
           steering_speed  = 0.7f;
           max_steering = 4.2f;
           steer_friction = 0.1f;

       }
       
        velocity = Vector2.Zero;
        
        moving = false;
        drifting = false;
        
        rotation = 0;
        texture = LoadTextureFromImage(img);
        size = new Vector2(texture.Width, texture.Height);


        steering = 0f;

    }

    public void Update(float dt)
    {
        if (rotation >= 360)
        {
            rotation -= 360;
            radians = Math.PI * Math.Ceiling(rotation) / 180;
        }

        if (rotation <= -360)
        {
            rotation += 360;
            radians = Math.PI * Math.Ceiling(rotation) / 180;
        }
        radians = Math.PI * Math.Ceiling(rotation) / 180;

        if (IsKeyDown(KeyboardKey.Right))
        {
            steering += 10f * dt * velocity.Y;
            

        }
        else if (IsKeyDown(KeyboardKey.Left))
        {
            steering -= 10f * dt * velocity.Y;
            
           
        }
        
        if (steering > max_steering)
            steering = max_steering;
        if (steering < -max_steering)
            steering = -max_steering;
        
        //velocity.Y += drift_angle * 2f * float.Sign(velocity.Y) * -1;

        steering *= (1 - steer_friction);
        rotation += steering;
        // basic steering
        
       // rotation = Clamp_Rotation(rotation);
      //  Console.WriteLine("Rotation: "+rotation);


        if (IsKeyDown(KeyboardKey.Up))
        {
            velocity.Y += aceeleration * dt;
            
        }
        else if (IsKeyDown(KeyboardKey.Down))
        {
            velocity.Y -= aceeleration * dt;
            
        }

        if (IsKeyDown(KeyboardKey.Space))
        {
            if (velocity.Y > 0.01)
            {
                
                velocity.Y -= aceeleration  * dt;
                
            }
            else if (velocity.Y < -0.01)
            { 
                
                velocity.Y += aceeleration  * dt;
                
            }
        }
        
        if (velocity.Y != 0)
        {
            if (velocity.Y > 0)
                velocity.Y -= friction * dt;
            if (velocity.Y < 0)
                velocity.Y += friction * dt;
        }
        
        if (velocity.Y > max_velocity)
            velocity.Y = max_velocity;
        
        if (velocity.Y < -max_velocity)
            velocity.Y = -max_velocity;

        
       // radians = Math.PI * Math.Ceiling(rotation) / 180;
        
        
        pos.Y -= (float)Math.Sin(radians) * velocity.Y * dt;
        pos.X -= (float)Math.Cos(radians) * velocity.Y * dt;
        
        // drifting
        drift_angle =(float)(rotation + drift_angle * drift_bias) / (1 + drift_bias);
        radians = Math.PI * drift_angle / 180;

        if (drift_angle > 0.5f || drift_angle < -0.5f)
            drifting = true;
        else
            drifting = false;
        
       // Console.WriteLine("Drifting: "+ drifting);
      

        //velocity.Y += drift_angle * Math.Sign(velocity.Y);
        //velocity.Y = float.Clamp(velocity.Y, 0, max_velocity);
        
        pos.Y -= (float)Math.Sin(radians) * velocity.Y * dt;
        pos.X -= (float)Math.Cos(radians) * velocity.Y * dt;
        
        
    }

    public void Draw(Camera2D cam)
    {
        
        DrawTexturePro(texture, new Rectangle(0, 0, texture.Width, texture.Height), new Rectangle(pos.X, pos.Y, size.X, size.Y), new Vector2(size.X/2 , size.Y/2), (float)rotation- 90, Color.White );
        //DrawRectangleLines((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y, Color.Red);
        //DrawRectanglePro(rect, new Vector2(rect.Width/2, rect.Height/2), (float)rotation, Color.Beige);
        
    }

    public void DrawUI()
    {
        DrawText("Speed: "+ Math.Floor(velocity.Y), 10, 980, 32, Color.Red );
        DrawText("Rotation: "+ Math.Ceiling(rotation), 10, 1040, 32, Color.Red );
        rlImGui.Begin();
        
        ImGui.Begin("Values".AsSpan());
        //ImGui.SetWindowSize(new Vector2(400, 300));
        ImGui.SetWindowFontScale(2);
        ImGui.SliderFloat("speed".AsSpan(), ref aceeleration, 0f, max_velocity);
        ImGui.Text("Speed: " + Math.Floor(velocity.Y));
        ImGui.Text("Rotation: " + Math.Ceiling(rotation));
        ImGui.Text("Radians: " + Math.Ceiling(radians));
       // ImGui.SliderFloat("max_speed".AsSpan(), ref max_velocity, 0f, 1000f);
       // ImGui.SliderFloat("friction".AsSpan(), ref friction, 0f, 100f);
       // ImGui.SliderFloat("drift_bias".AsSpan(), ref drift_bias, 0f, 50f);
       // ImGui.SliderFloat("steering_speed".AsSpan(), ref steering_speed, 0f, 10f);
      //  ImGui.SliderFloat("max_steering".AsSpan(), ref max_steering, 0f, 100f);
      //  ImGui.SliderFloat("steering_friction".AsSpan(), ref steer_friction, 0f, 2f);
        
        
        ImGui.End();
        rlImGui.End();
        
       
        
        
    }


    public double Clamp_Rotation(double rot)
    {
        //rot = double.Clamp(rot, -360, 360);
        //rot = double.Floor(rot);
        if (double.Ceiling(rot) > 360 || double.Ceiling(rot) < -360)
        {
            //while (double.Floor(rot) >= 360 || double.Floor(rot) <= -360)
            if (double.Ceiling(rot) > 360) 
                rot = 0;
            else if (double.Ceiling(rot) < -360) 
                rot = 360;
            //}
        }

        return rot;
    }

}