using UnityEngine;

public class ShipTerminal : MonoBehaviour
{
    public Ship ship;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Response OnCommandRecieved(string recieved_data)
    {
        return Response.GenericResponse("Ship command recieved");
    }
    
}
