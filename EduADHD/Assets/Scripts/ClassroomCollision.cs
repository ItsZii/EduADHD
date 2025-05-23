using UnityEngine;
public class ClassroomCollision : MonoBehaviour
{
    private bool _playerInBoyView = false;
    public bool PlayerInBoyView => _playerInBoyView;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "boy")
        {
            _playerInBoyView = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "boy")
        {
            _playerInBoyView = false;
        }
    }
}
