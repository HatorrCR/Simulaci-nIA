using UnityEngine;
using System.Collections.Generic;

namespace IAEngine.Movement
{
    public enum MovementType
    {
        None,
        Seek,
        Flee,
        Arrive,
        Wander,
        Pursue,
        Evade,
        Align,
        Face,
        LookWhereYoureGoing,
        VelocityMatching,
        AutoPathFollow,
        Separation,
        Attraction,
        SeekWithObstacleAvoidance,
        SeekWithCollisionAvoidance
    }

    [RequireComponent(typeof(Kinematic))]
    public class AgentController : MonoBehaviour
    {
        public MovementType currentBehavior = MovementType.None;
        private MovementType lastBehavior = MovementType.None;

        [Header("Targets y configuración")]
        public Transform target;
        public Kinematic targetKinematic;
        public Path path;
        public Kinematic[] group;

        private Kinematic agent;
        private Renderer agentRenderer;
        private LookWhereYoureGoing looker;

        private Color[] behaviorColors = new Color[] {
            Color.white, Color.green, Color.red, Color.yellow, Color.cyan,
            Color.magenta, Color.blue, Color.grey, Color.black, Color.gray,
            Color.white * 0.5f, Color.red * 0.5f, Color.green * 0.5f,
            Color.blue * 0.5f, Color.yellow * 0.5f, Color.cyan * 0.5f, Color.magenta * 0.5f,
            Color.white * 0.8f
        };

        void Start()
        {
            agent = GetComponent<Kinematic>();
            agentRenderer = GetComponentInChildren<Renderer>();
            looker = gameObject.AddComponent<LookWhereYoureGoing>();
            looker.agent = agent;
            ApplyColor();
        }

        void Update()
        {
            HandleKeyInput();
            ApplyBehavior();
        }

        void HandleKeyInput()
        {
            if (Input.GetKeyDown(KeyCode.Alpha0)) currentBehavior = MovementType.None;
            if (Input.GetKeyDown(KeyCode.Alpha1)) currentBehavior = MovementType.Seek;
            if (Input.GetKeyDown(KeyCode.Alpha2)) currentBehavior = MovementType.Flee;
            if (Input.GetKeyDown(KeyCode.Alpha3)) currentBehavior = MovementType.Arrive;
            if (Input.GetKeyDown(KeyCode.Alpha4)) currentBehavior = MovementType.Wander;
            if (Input.GetKeyDown(KeyCode.Alpha5)) currentBehavior = MovementType.Pursue;
            if (Input.GetKeyDown(KeyCode.Alpha6)) currentBehavior = MovementType.Evade;
            if (Input.GetKeyDown(KeyCode.Alpha7)) currentBehavior = MovementType.Align;
            if (Input.GetKeyDown(KeyCode.Alpha8)) currentBehavior = MovementType.Face;
            if (Input.GetKeyDown(KeyCode.Alpha9)) currentBehavior = MovementType.LookWhereYoureGoing;

            if (Input.GetKeyDown(KeyCode.Q)) currentBehavior = MovementType.VelocityMatching;
            if (Input.GetKeyDown(KeyCode.W)) currentBehavior = MovementType.AutoPathFollow;
            if (Input.GetKeyDown(KeyCode.E)) currentBehavior = MovementType.Separation;
            if (Input.GetKeyDown(KeyCode.R)) currentBehavior = MovementType.Attraction;
            if (Input.GetKeyDown(KeyCode.I)) currentBehavior = MovementType.SeekWithObstacleAvoidance;
            if (Input.GetKeyDown(KeyCode.U)) currentBehavior = MovementType.SeekWithCollisionAvoidance;
        }

        void ApplyBehavior()
        {
            if (currentBehavior != lastBehavior)
            {
                ApplyColor();
                lastBehavior = currentBehavior;
            }

            SteeringOutput steering = null;

            switch (currentBehavior)
            {
                case MovementType.Seek:
                    Seek seek = gameObject.AddComponent<Seek>();
                    seek.agent = agent;
                    seek.target = target;
                    steering = seek.GetSteering();
                    Destroy(seek);
                    break;
                case MovementType.Flee:
                    Flee flee = gameObject.AddComponent<Flee>();
                    flee.agent = agent;
                    flee.target = target;
                    steering = flee.GetSteering();
                    Destroy(flee);
                    break;
                case MovementType.Arrive:
                    Arrive arrive = gameObject.AddComponent<Arrive>();
                    arrive.agent = agent;
                    arrive.target = target;
                    steering = arrive.GetSteering();
                    Destroy(arrive);
                    break;
                case MovementType.Wander:
                    Wander wander = gameObject.AddComponent<Wander>();
                    wander.agent = agent;
                    steering = wander.GetSteering();
                    Destroy(wander);
                    break;
                case MovementType.Pursue:
                    Pursue pursue = gameObject.AddComponent<Pursue>();
                    pursue.agent = agent;
                    pursue.target = targetKinematic;
                    steering = pursue.GetSteering();
                    Destroy(pursue);
                    break;
                case MovementType.Evade:
                    Evade evade = gameObject.AddComponent<Evade>();
                    evade.agent = agent;
                    evade.target = targetKinematic;
                    steering = evade.GetSteering();
                    Destroy(evade);
                    break;
                case MovementType.Align:
                    Align align = gameObject.AddComponent<Align>();
                    align.agent = agent;
                    align.target = target;
                    steering = align.GetSteering();
                    Destroy(align);
                    break;
                case MovementType.Face:
                    Face face = gameObject.AddComponent<Face>();
                    face.agent = agent;
                    face.faceTarget = target;
                    face.target = target;
                    steering = face.GetSteering();
                    Destroy(face);
                    break;
                case MovementType.LookWhereYoureGoing:
                    break;
                case MovementType.VelocityMatching:
                    VelocityMatching vm = gameObject.AddComponent<VelocityMatching>();
                    vm.agent = agent;
                    vm.target = targetKinematic;
                    steering = vm.GetSteering();
                    Destroy(vm);
                    break;
                case MovementType.AutoPathFollow:
                    AutoPathFollower auto = gameObject.AddComponent<AutoPathFollower>();
                    auto.path = path;
                    auto.speed = agent.maxSpeed;
                    steering = null;
                    break;
                case MovementType.Separation:
                    Separation sep = gameObject.AddComponent<Separation>();
                    sep.agent = agent;
                    sep.targets = new List<Kinematic>(group);
                    steering = sep.GetSteering();
                    Destroy(sep);
                    break;
                case MovementType.Attraction:
                    Attraction attr = gameObject.AddComponent<Attraction>();
                    attr.agent = agent;
                    attr.targets = new List<Kinematic>(group);
                    steering = attr.GetSteering();
                    Destroy(attr);
                    break;
                case MovementType.SeekWithObstacleAvoidance:
                    ObstacleAvoidance smartOA = gameObject.AddComponent<ObstacleAvoidance>();
                    smartOA.agent = agent;
                    smartOA.target = target;
                    smartOA.maxAcceleration = 4f;
                    smartOA.rayLength = 7f;
                    smartOA.sideStepDistance = 4f;
                    smartOA.tolerance = 0.5f;
                    smartOA.obstacleLayers = LayerMask.GetMask("Obstacles");
                    steering = smartOA.GetSteering();
                    Destroy(smartOA);
                    break;
                case MovementType.SeekWithCollisionAvoidance:
                    CollisionAvoidance smartCA = gameObject.AddComponent<CollisionAvoidance>();
                    smartCA.agent = agent;
                    smartCA.target = target;
                    smartCA.maxAcceleration = 4f;
                    smartCA.rayLength = 7f;
                    smartCA.sideStepDistance = 4f;
                    smartCA.tolerance = 0.5f;
                    smartCA.detectionLayers = LayerMask.GetMask("Default");
                    smartCA.enemyTag = "Enemy";
                    steering = smartCA.GetSteering();
                    Destroy(smartCA);
                    break;
            }

            if (steering != null)
            {
                agent.ApplySteering(steering, Time.deltaTime);
            }
        }

        void ApplyColor()
        {
            if (agentRenderer != null && (int)currentBehavior < behaviorColors.Length)
            {
                agentRenderer.material.color = behaviorColors[(int)currentBehavior];
            }
        }

        void OnDrawGizmos()
        {
            if (agent == null) return;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + agent.velocity);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.forward * 2f);
        }
    }
}
