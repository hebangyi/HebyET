using Box2DSharp;
using Testbed.Abstractions;

namespace Testbed.Samples.Bodies;


/// This is a test of typical character collision scenarios. This does not
/// show how you should implement a character in your application.
/// Instead this is used to test smooth collision on chain shapes.
[Sample("GameTest", "GameTest")]
public class GameTest : SampleBase
{
    BodyId _circleCharacterId;

    BodyId _capsuleCharacterId;

    BodyId _boxCharacterId;

    public GameTest(Settings settings)
        : base(settings)
    {
        if (settings.Restart == false)
        {
            Global.Camera.Center = (-2.0f, 7.0f);
            Global.Camera.Zoom = 25.0f * 0.4f;
        }
        
        // Ground body
        {
            BodyDef bodyDef = BodyDef.DefaultBodyDef();
            BodyId groundId = Body.CreateBody(WorldId, bodyDef);

            ShapeDef shapeDef = ShapeDef.DefaultShapeDef();
            Segment segment = ((-20.0f, 0.0f), (20.0f, 0.0f));
            Shape.CreateSegmentShape(groundId, shapeDef, segment);
        }

        // static Box
        {
            BodyDef bodyDef = BodyDef.DefaultBodyDef();
            bodyDef.Type = BodyType.DynamicBody;
            BodyId bodyId = Body.CreateBody(WorldId, bodyDef);
            
            Polygon box = Geometry.MakeOffsetBox(1.0f, 3.0f, (-1.0f, 3.0f), Rot.Identity);
            ShapeDef shapeDef = ShapeDef.DefaultShapeDef();
            shapeDef.EnableHitEvents = true;
            
            Shape.CreatePolygonShape(bodyId, shapeDef, box);
        }


        // Player
        {
            BodyDef bodyDef = BodyDef.DefaultBodyDef();
            bodyDef.Type = BodyType.KinematicBody;
            BodyId bodyId = Body.CreateBody(WorldId, bodyDef);
            
            Polygon box = Geometry.MakeOffsetBox(1.0f, 1.0f, (4.0f, 3.0f), Rot.Identity);
            ShapeDef shapeDef = ShapeDef.DefaultShapeDef();
            shapeDef.EnableHitEvents = true;
            
            Shape.CreatePolygonShape(bodyId, shapeDef, box);
            Body.SetLinearVelocity(bodyId, (-1.0f, 0f));
        }
    }

    public override void UpdateUI()
    {
        DrawString("This tests various character collision shapes.");
        DrawString("Limitation: square and hexagon can snag on aligned boxes.");

        World.GetSensorEvents(WorldId);
        
        var events = World.GetContactEvents(WorldId);
        DrawString($"ContactEvents Count: {events.HitCount}");

        if (events.HitCount > 0)
        {
            Console.WriteLine("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        }
        
        var sensorEvents = World.GetSensorEvents(WorldId);
        DrawString($"SensorEvent {sensorEvents.BeginEvents.Length} {sensorEvents.EndEvents.Length}");

        var bodyEvents = World.GetBodyEvents(WorldId);
        DrawString($"BodyEvent {bodyEvents.MoveEvents.Length}");
        
        base.UpdateUI();
    }
}