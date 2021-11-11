using System.Collections;
using UnityEngine;

public class Bulletspawn : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float shootInterval;
    public float shootForce;
    public Transform shootPos;

    bool canShoot = true;

    private void Update()
    {
        if (Input.GetMouseButton(0) && canShoot == true)
        {
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        canShoot = false;

        GameObject bulletInstance = Instantiate(bulletPrefab, shootPos.position, shootPos.rotation);
        bulletInstance.GetComponent<Rigidbody>().AddForce(shootForce * shootPos.up, ForceMode.Impulse);
      // bulletInstance.transform.SetPositionAndRotation(shootPos.position, shootPos.rotation);
        yield return new WaitForSeconds(shootInterval);
        canShoot = true;
    }
}
