using UnityEngine;

public class PlantModel : MonoBehaviour
{
    public string plantName;
    public Renderer seedlingModel;
    public Renderer youngModel;
    public Renderer matureModel;
    public Renderer witheredModel;


    // Start is called before the first frame update
    void Start()
    {
        seedlingModel.enabled = false;
        youngModel.enabled = false;
        matureModel.enabled = false;
        witheredModel.enabled = false;
    }

    public void SetPhase(PlantPhase phase)
    {
        Debug.Log("hio");
        seedlingModel.enabled = false;
        youngModel.enabled = false;
        matureModel.enabled = false;
        witheredModel.enabled = false;

        switch (phase)
        {
            case PlantPhase.seedling:
                seedlingModel.enabled = true;
                break;
            case PlantPhase.young:
                youngModel.enabled = true;
                break;
            case PlantPhase.mature:
                matureModel.enabled = true;
                break;
            case PlantPhase.withered:
                witheredModel.enabled = true;
                break;
        }
    }
}
