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
        public LineRenderer stupid;
        public void Awake() {
            stupid = base.gameObject.GetComponent<LineRenderer>();
            //.loop = true;
            stupid.material = Engineer.Engineer.green;

        }
        public void FixedUpdate() {
            stupid.enabled = true;
        }
    }
}
