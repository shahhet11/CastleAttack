using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Machine : MonoBehaviour
{
    public Animator animator;
    public enum WeaponType { Rock, FireRock, Arrow, FireArrow, Cannon, M777A2, Panhard, Rocket, Tank, Drone, TwinTank, Robot, Helicopter, SpaceShip14, SpaceShip15 };
    public WeaponType weaponType;

    public void ShootProjectile()
    {
        StartCoroutine(ThrowBall(.5f, 0));
    }

    public IEnumerator ThrowBall(float delay, int value)
    {
        animator.SetInteger("Catapult", 1);
       
        if (weaponType == WeaponType.FireRock || weaponType == WeaponType.Rock || weaponType == WeaponType.Rocket)// || weaponType==WeaponType.)//Rock
        {
            GameManager.instance.IsDustParticles = true;
            yield return new WaitForSeconds(delay);
        }

        CharacterAttack.instance.GamePlayThrowAnimation();

        if (weaponType == WeaponType.Cannon)//Canon
        {
            yield return new WaitForSeconds(delay / 10);

            Instantiate(EnemyManager.insance.DustSpawn,
                GameManager.instance.ThrowableSpawnPosition[(int)MachineryManager.instance._WeaponType].position,
                GameManager.instance.ThrowableSpawnPosition[(int)MachineryManager.instance._WeaponType].rotation);
        }

        if ((int)weaponType > 1) //Except Rock and Fire Rock
        {
            yield return new WaitForSeconds(delay);
        }

        animator.SetInteger("Catapult", value);
    }
}
