using UnityEngine;

public class Ship : MonoBehaviour
{
    private uint bits;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    // public BuildBotResponse HandleBuildBotCommand(BuildBotCommand cmd) 

    public ShipBitsQueryResponse HandleGetBitsQuery(ShipBitsQuery query)
    {
        ShipBitsQueryResponse response = new ShipBitsQueryResponse();
        response.bits = bits;
        return response;
    }
}
