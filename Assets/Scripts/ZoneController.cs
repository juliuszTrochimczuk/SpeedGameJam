using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ZoneController : MonoBehaviour
{
    [SerializeField] private Rigidbody zone1;
    [SerializeField] private Rigidbody zone2;
    [SerializeField] private int hideDistance=-200;
    
    private bool zone1Coached=false;
    private bool zone2Coached=false;
    

    public void CollapseZone1() {
        zone1Coached=true;
    }
    
    public void CollapseZone2() {
        zone2Coached=true;
    }

    private void FixedUpdate() {
        if (zone1Coached && zone1.position.y > hideDistance) {
            zone1.MovePosition(new Vector3(zone1.position.x, zone1.position.y-(0.05f/Time.deltaTime), zone1.position.z));
        }
        
        if (zone2Coached && zone2.position.y > hideDistance) {
            zone2.MovePosition(new Vector3(zone2.position.x, zone2.position.y-(0.05f/Time.deltaTime), zone2.position.z));
        }
    }
}
