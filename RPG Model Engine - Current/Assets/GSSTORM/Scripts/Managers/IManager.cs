using UnityEngine;
using System.Collections;

namespace GSStorm.RPG.Engine
{
    public interface IManager
    {
        ManagerStatus Status { get;}

        IEnumerator PreLaunch();
        IEnumerator StartLaunch();
        IEnumerator PostLaunch();
    }
}
