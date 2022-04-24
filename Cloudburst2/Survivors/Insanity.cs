using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cloudburst.Survivors
{
    public class Insanity : MonoBehaviour
    {
        public void Awake() {
            base.gameObject.GetComponent<LineRenderer>().loop = true;
            base.gameObject.GetComponent<LineRenderer>().material = Engineer.Engineer.green;

        }
    }
}
