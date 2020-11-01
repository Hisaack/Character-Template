﻿using UnityEngine;

namespace Playcraft
{
    public class ExplosionForce : MonoBehaviour
    {
        [Tooltip("Range of objects affected by explosion")]
        [SerializeField] float range = 5;
        
        [Tooltip("Base value for calculated strength of explosion")]
        [SerializeField] float strength = 500;

        public void Explode()
        {
            var nearbyColliders = Physics.OverlapSphere(transform.position, range);
            
            foreach (var other in nearbyColliders)
            {
                Rigidbody rb = other.GetComponent<Rigidbody>();
                if (!rb) continue;
                
                var selfToOther = other.transform.position - transform.position;
                var direction = selfToOther.normalized;
                var distance = selfToOther.magnitude;
                var percent = distance / range;
                var inversePercent = 1 - percent;
                var force = strength * inversePercent;
                rb.AddForce(direction * force);
            }
        }
        
        void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}
