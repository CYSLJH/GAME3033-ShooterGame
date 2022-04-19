using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    // interface, more of a way organizing than it is a purpose.  Useful for working in a team with other people.
    // serves the function of making sure you get everything you need in order to work properly

    void TakeDamage(float damage);
    void Destroy();
}
