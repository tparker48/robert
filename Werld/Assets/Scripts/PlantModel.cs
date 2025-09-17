using UnityEngine;

public class PlantModel : MonoBehaviour
{
    public string plantName;
    public GameObject seedlingModel;
    public GameObject youngModel;
    public GameObject matureModel;
    public GameObject witheredModel;


    // Start is called before the first frame update
    void Start()
    {
        seedlingModel.SetActive(false);
        seedlingModel.SetActive(false);
        youngModel.SetActive(false);
        matureModel.SetActive(false);
        witheredModel.SetActive(false);
    }

    public void SetPhase(PlantPhase phase)
    {
        seedlingModel.SetActive(false);
        youngModel.SetActive(false);
        matureModel.SetActive(false);
        witheredModel.SetActive(false);

        switch (phase)
        {
            case PlantPhase.seedling:
                seedlingModel.SetActive(true);
                break;
            case PlantPhase.young:
                youngModel.SetActive(true);
                break;
            case PlantPhase.mature:
                matureModel.SetActive(true);
                break;
            case PlantPhase.withered:
                witheredModel.SetActive(true);
                break;
        }
    }
}
