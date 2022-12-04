using System.Collections;
using System.Collections.Generic;
using System;
using System.Globalization;
using UnityEngine;
using Accord.Math.Decompositions;
using UnityEngine.UI;

public class Part2Operations : MonoBehaviour
{
    public Image image;
    public GameObject backButton;
    public GameObject nextButton;
    public GameObject teapot;
    public GameObject origin;

    private int currentPicture=0;
    private double[,] refCenter=new double[1,3] { {2313,886,1} };
    private double refScaleXZ=600;
    private double refScaleY;
    private double refZ=-20;
    private Vector3 refTransformPosition;
    private Vector3 refRotation=new Vector3(-46.897f, -43.527f, 37.756f);

    private double[,] refScenePoints=new double[5,2] { {2380,812}, {2115,1309}, {989,832}, {2172,831}, {1739,511} };
    private double[,] im1Points=new double[5,2] { {1903,674}, {2109,928}, {1294,919}, {1835,736}, {1530,772} };
    private double[,] im2Points=new double[5,2] { {2130,771}, {2266,1019}, {1515,1225}, {2052,822}, {1756,835} };
    private double[,] im3Points=new double[5,2] { {2825,869}, {2715,1181}, {2112,1127}, {2701,896}, {2420,800} };
    private double[,] im4Points=new double[5,2] { {2962,776}, {2750,1143}, {2161,1008}, {2809,800}, {2512,653} };
    private double[,] im5Points=new double[5,2] { {2504,598}, {2472,979}, {1648,1018}, {2367,650}, {2002,579} };
    private double[,] im6Points=new double[5,2] { {2525,549}, {2549,1037}, {1625,931}, {2390,628}, {2001,556} };
    private double[,] im7Points=new double[5,2] { {2048,565}, {2181,1013}, {1124,1097}, {1932,650}, {1521,625} };
    private double[,] im8Points=new double[5,2] { {1863,608}, {2072,985}, {944,1292}, {1754,690}, {1318,711} };
    private double[,] im9Points=new double[5,2] { {1882,704}, {1983,1017}, {1203,1108}, {1795,760}, {1488,740} };
    private double[,] im10Points=new double[5,2] { {1965,830}, {2046,1047}, {1427,1167}, {1894,866}, {1648,852} };
    private double[,] im11Points=new double[5,2] { {2658,207}, {2618,833}, {1493,698}, {2475,307}, {1985,198} };
    private double[,] im12Points=new double[5,2] { {2232,807}, {1781,1398}, {475,363}, {1995,774}, {1571,254} };
    private double[,] im13Points=new double[5,2] { {1556,991}, {1816,1418}, {411,1763}, {1439,1079}, {936,1082} };
    private double[,] im14Points=new double[5,2] { {1911,897}, {2210,1294}, {758,1838}, {1780,992}, {1233,1043} };
    private double[,] im15Points=new double[5,2] { {2071,729}, {2180,1231}, {1026,1285}, {1937,818}, {1487,775} };
    private double[,] im16Points=new double[5,2] { {2205,890}, {2014,1382}, {1037,1054}, {2030,913}, {1634,681} };
    private double[,] im17Points=new double[5,2] { {2676,1028}, {2453,1421}, {1753,1282}, {2510,1048}, {2175,873} };
    private double[,] im18Points=new double[5,2] { {2247,750}, {2137,1155}, {1133,1121}, {2082,781}, {1669,630} };
    
    // Start is called before the first frame update
    void Start()
    {
        backButton.SetActive(false);
        refTransformPosition=new Vector3((float)refCenter[0,0], (float)refCenter[0,1], (float)refZ);
        int pixelOfCable=754;
        int lengthOfCable=15;
        int lengthOfBox=lengthOfCable*pixelOfCable/15;
        refScaleY=5000*lengthOfBox/3264;
    }

    public void goBack()
    {
        if(currentPicture==18)
            nextButton.SetActive(true);
        else if(currentPicture==1)
            backButton.SetActive(false);

        currentPicture--;

        if(currentPicture==0)
            image.sprite = Resources.Load<Sprite>("ImagesPart2/Ref");
        else
            image.sprite = Resources.Load<Sprite>("ImagesPart2/"+currentPicture);

        redoTeapotPlacemant(currentPicture);
    }

    public void goNext()
    {
        if(currentPicture==0)
            backButton.SetActive(true);
        else if(currentPicture==17)
            nextButton.SetActive(false);

        currentPicture++;

        string pathName="ImagesPart2/"+currentPicture;
        image.sprite = Resources.Load<Sprite>(pathName);

        redoTeapotPlacemant(currentPicture);
    }

    private void redoTeapotPlacemant(int imageNo)
    {
        double[,] point=new double[3,1];
        point[0,0]=refCenter[0,0];
        point[1,0]=refCenter[0,1];
        point[2,0]=1;

        if(imageNo==0)
        {
            teapot.transform.localPosition=refTransformPosition;
            teapot.transform.localScale=new Vector3((float)refScaleXZ, (float)refScaleY, (float)refScaleXZ);
            teapot.transform.rotation=Quaternion.Euler(refRotation.x, refRotation.y, refRotation.z);
        }
        else if(imageNo==1)
            applyObjectProjection(im1Points, point);
        else if(imageNo==2)
            applyObjectProjection(im2Points, point);
        else if(imageNo==3)
            applyObjectProjection(im3Points, point);
        else if(imageNo==4)
            applyObjectProjection(im4Points, point);
        else if(imageNo==5)
            applyObjectProjection(im5Points, point);
        else if(imageNo==6)
            applyObjectProjection(im6Points, point);
        else if(imageNo==7)
            applyObjectProjection(im7Points, point);
        else if(imageNo==8)
            applyObjectProjection(im8Points, point);
        else if(imageNo==9)
            applyObjectProjection(im9Points, point);
        else if(imageNo==10)
            applyObjectProjection(im10Points, point);
        else if(imageNo==11)
            applyObjectProjection(im11Points, point);
        else if(imageNo==12)
            applyObjectProjection(im12Points, point);
        else if(imageNo==13)
            applyObjectProjection(im13Points, point);
        else if(imageNo==14)
            applyObjectProjection(im14Points, point);
        else if(imageNo==15)
            applyObjectProjection(im15Points, point);
        else if(imageNo==16)
            applyObjectProjection(im16Points, point);
        else if(imageNo==17)
            applyObjectProjection(im17Points, point);
        else if(imageNo==18)
            applyObjectProjection(im18Points, point);
    }

    public void applyObjectProjection(double[,] objectPoints, double[,] point)
    {
        //Find center
        double[,] imHmMatrix=calcHomography(refScenePoints, objectPoints);
        double[,] imCenter=applyProjection(point, imHmMatrix);

        //Find scale
        double realDistance=Distance(refScenePoints[0,0], refScenePoints[0,1], refScenePoints[1,0], refScenePoints[1,1]);
        double im1Distance=Distance(objectPoints[0,0], objectPoints[0,1], objectPoints[1,0], objectPoints[1,1]);
        double scaling=im1Distance/realDistance;

        //Find rotation
        Vector3 refDir=(new Vector3((float)refScenePoints[3,0], (float)refScenePoints[3,1], 0))-(new Vector3((float)refCenter[0,0], (float)refCenter[0,1], 0));
        Vector3 projDir=(new Vector3((float)objectPoints[3,0], (float)objectPoints[3,1], 0))-(new Vector3((float)imCenter[0,0], (float)imCenter[1,0], 0));
        float angle = Vector2.SignedAngle(projDir, refDir);

        //Apply
        teapot.transform.localPosition=new Vector3((float)imCenter[0,0], (float)imCenter[1,0], (float)refZ);
        teapot.transform.localScale=new Vector3((float)(refScaleXZ*scaling), (float)(refScaleY*scaling), (float)(refScaleXZ*scaling));
        teapot.transform.rotation=Quaternion.Euler(refRotation.x, refRotation.y, refRotation.z);
        teapot.transform.Rotate(0f, angle, 0f, Space.Self);
    }

    public double[,] calcHomography(double[,] sourceM, double[,] destinationM)
    {
        double[,] result=new double[3,3];
        int rowCount=sourceM.GetLength(0);
        double[,] matrix=new double[rowCount*2, 9];

        int pointNo=0;
        for (int i=0; i<rowCount*2; ++i)
        {
            if (i%2==0)
            {
                matrix[i,0]=-1*sourceM[pointNo, 0];
                matrix[i,1]=-1*sourceM[pointNo, 1];
                matrix[i,2]=-1;
                matrix[i,3]=0;
                matrix[i,4]=0;
                matrix[i,5]=0;
                matrix[i,6]=sourceM[pointNo, 0]*destinationM[pointNo, 0];
                matrix[i,7]=destinationM[pointNo, 0]*sourceM[pointNo, 1];
                matrix[i,8]=destinationM[pointNo, 0];
            }
            else
            {
                matrix[i,0]=0;
                matrix[i,1]=0;
                matrix[i,2]=0;
                matrix[i,3]=-1*sourceM[pointNo, 0];
                matrix[i,4]=-1*sourceM[pointNo, 1];
                matrix[i,5]=-1;
                matrix[i,6]=sourceM[pointNo, 0]*destinationM[pointNo, 1];
                matrix[i,7]=destinationM[pointNo, 1]*sourceM[pointNo, 1];
                matrix[i,8]=destinationM[pointNo, 1];
                pointNo++;
            }
        }

        SingularValueDecomposition svd= new SingularValueDecomposition(matrix, true, true);
        double[,] VT=TransposeD(svd.RightSingularVectors);

        int count=0;
        for(int i=0; i<3; i++)
            for(int j=0; j<3; j++)
            {
                result[i,j]=VT[8, count];
                count++;
            }

        return result;
    }

    public double[,] projection(double[,] sourceM, double[,] hm)
    {
        return MultiplyMatrixD(hm, sourceM);
    }

    public double[,] applyProjection(double[,] sourceM, double[,] hm)
    {
        double[,] projectionResult=projection(sourceM, hm);
        for(int i=0; i<3; ++i)
            projectionResult[i,0]/=projectionResult[2,0];

        return projectionResult;
    }

    private double Distance(double x1, double y1, double x2, double y2)
    {
        return Math.Sqrt(((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2))); 
    }

    private double Determinant3DD(double[,] arr1)
    {
        double det=0;
        for(int i=0; i<3; i++)
        {
            det = det + (arr1[0,i]*(arr1[1,(i+1)%3]*arr1[2,(i+2)%3] - arr1[1,(i+2)%3]*arr1[2,(i+1)%3]));
        }

        return det;
    }

    //Only works on suitable matrices
    private double[,] MultiplyMatrixD(double[,] A, double[,] B)
    {
        int rA = A.GetLength(0);
        int cA = A.GetLength(1);
        int rB = B.GetLength(0);
        int cB = B.GetLength(1);

        double[,] kHasil = new  double[rA, cB];

        double temp = 0;

        for (int i = 0; i < rA; i++)
        {
            for (int j = 0; j < cB; j++)
            {
                temp = 0;
                for (int k = 0; k < cA; k++)
                {
                    temp += A[i, k] * B[k, j];
                }
                kHasil[i, j] = temp;
            }
        }

        return kHasil;
    }

    private double[,] TransposeD(double[,] matrix)
    {
        int w = matrix.GetLength(0);
        int h = matrix.GetLength(1);

        double[,] result = new double[h, w];

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                result[j, i] = matrix[i, j];
            }
        }

        return result;
    }

    public void printArrayD(double[,] array)
    {
        int w =  array.GetLength(0);
        int h =  array.GetLength(1);

        for(int i=0; i<w; ++i)
        {
            string printMessage="";
            for(int j=0; j<h; ++j)
                printMessage=printMessage+", "+array[i,j].ToString();
            print(printMessage);
        }
    }
}
