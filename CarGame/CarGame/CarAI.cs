using System.Numerics;
using System.Runtime.CompilerServices;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace CarGame;

public class CarAI
{
    private Vector2 pos;
    private Vector2 size;
    private Texture2D texture;
    public Vector2 target;
    private float rotation;


    private int num_rays;

    private List<Vector2> ray_dirs; 
    private List<Vector2> intrest_dir;
    private List<Vector2> danger_dir;

    private Vector2 chosen_dir;
    private Vector2 velocity;
    private Vector2 acceleration;
    

    public CarAI(Vector2 _pos, Texture2D tex, Vector2 target_pos, int _num_rays)
    {
        pos = _pos;
        texture = tex;
        size = new Vector2(tex.Width, tex.Height);
        target = target_pos;
        rotation = 0;

        num_rays = _num_rays;
        
        intrest_dir = new List<Vector2>(num_rays);
        danger_dir = new List<Vector2>(num_rays);
        
        ray_dirs = new List<Vector2>(num_rays);
        for (int i = 0; i < ray_dirs.Count; i++)
        {
            float angle = i * 2 * (float)Math.PI / num_rays;

        }

        
        
        chosen_dir = Vector2.Zero;
        velocity = Vector2.Zero;
        acceleration = Vector2.Zero;

    }

    public void Update(float dt)
    {
        
    }




}