using UnityEngine;

public class ShipTerminal : MonoBehaviour
{
    private Ship ship;

    public void Start()
    {
        ship = Ship.Instance;
    }

    public Response OnCommandRecieved(string rawCmd)
    {
        ShipCommand cmd = CommandParser.Parse<ShipCommand>(rawCmd);
        switch (cmd.cmd_id)
        {
            case BuyRobert.id:
                return HandleBuyRobert(rawCmd);
            case GetRobertCost.id:
                return HandleGetRobertCost(rawCmd);
            case AddFloor.id:
                return HandleAddFloor(rawCmd);
            case GetBits.id:
                return HandleGetBits(rawCmd);
            case GetNewFloorCost.id:
                return HandleGetNewFloorCost(rawCmd);
            case GetEquipmentCost.id:
                return HandleGetEquipmentCost(rawCmd);
            case GetNumFloors.id:
                return HandleGetNumFloors(rawCmd);
        }
        return Response.ErrorResponse("Unrecognized cmd_id for Ship Command!");
    }

    private Response HandleBuyRobert(string rawCmd)
    {
        BuyRobert cmd = CommandParser.Parse<BuyRobert>(rawCmd);
        RobertType typeToBuy = RobertTypes.Lookup(cmd.type);
        if (ship.BuyRobert(typeToBuy))
        {
            return Response.GenericResponse("Successfully Purchased!");
        }
        else
        {
            return Response.GenericResponse("Purchase Failed!");
        }

    }

    private Response HandleAddFloor(string _)
    {
        if (ship.AddFloor())
        {
            return Response.GenericResponse("Floor Added!");
        }
        else
        {
            return Response.ErrorResponse("Failed to Add Floor!");
        }
        
    }

    private Response HandleGetRobertCost(string rawCmd)
    {
        GetRobertCost cmd = CommandParser.Parse<GetRobertCost>(rawCmd);
        RobertType typeToCheck = RobertTypes.Lookup(cmd.type);
        if (typeToCheck != null)
        {
            GetRobertCostResponse response = new GetRobertCostResponse();
            response.cost = (int)typeToCheck.initialCost;
            return response;
        }
        else
        {
            return Response.ErrorResponse("Invalid Robert Type!");
        }
    }

    private Response HandleGetEquipmentCost(string rawCmd)
    {
        // TODO
        GetEquipmentCost cmd = CommandParser.Parse<GetEquipmentCost>(rawCmd);
        GetEquipmentCostResponse response = new GetEquipmentCostResponse();
        return Response.ErrorResponse("Not Implemented!");
    }

    private Response HandleGetBits(string _)
    {
        GetBitsResponse response = new GetBitsResponse();
        response.bits = ship.bits;
        return response;
    }

    private Response HandleGetNewFloorCost(string _)
    {
        GetNewFloorCostResponse response = new GetNewFloorCostResponse();
        int cost = ship.GetNewFloorCost();
        if (cost >= 0)
        {
            response.cost = (uint)cost;
            return response;
        }
        else
        {
            return Response.ErrorResponse("Cannot Build More Floors!");
        }
    }

    private Response HandleGetNumFloors(string _)
    {
        GetNumFloorsResponse response = new GetNumFloorsResponse();
        response.floors = (uint)ship.floors.Count;
        return response;
    }

}