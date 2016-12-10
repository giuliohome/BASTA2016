using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaCPUReader
{

    /// <summary>
    /// The ActorSystem is the entrance Point used by the WPF Project to start the ActorSystem.
    /// It takes a delegate as a parameter that is used to update data in the UI.
    /// </summary>
    public class ActorSystemReference
    {
        private static ActorSystem _actorSystem;

        private static IActorRef coordActor;
        public static void CreateActorSystem(Action<float, DateTime> SetDataPointAction)
        {
            _actorSystem = ActorSystem.Create("akka");

            Action<float, DateTime> DataPointSetter = SetDataPointAction;
            var chartingActor = _actorSystem.ActorOf(Props.Create(() => new ChartingActor(DataPointSetter)));

            coordActor = _actorSystem.ActorOf(Props.Create(() => new CoordinatorActor(chartingActor)), "Coord");
        }
        public static void Start()
        {
            if (coordActor != null)
            {
                coordActor.Tell(new StartMsg());
            }
        }
        public static void Stop()
        {
            if (coordActor != null)
            {
                coordActor.Tell(new StopMsg());
            }
        }
    }
}
