using System.Collections;
using System.Collections.Generic;
using System;
using System.Globalization;
using UnityEngine;
using Accord.Math.Decompositions;
using System.Linq;

public class Part1Operations : MonoBehaviour
{
    public double[,] source = new double[4, 2] { { 93, 63 }, { 293, 868 }, { 1207, 998}, { 1218, 309} };
    public double[,] destination = new double[4, 2] { { -7, 0 }, { 3, -6 }, { 7, -4 }, { 3, 2 } };
    public double[,] hmMatrix;

    public double[,] scenePoints= new double[5,2] { {100,0}, {0,100}, {200,200}, {400,300}, {300,500} };
    public double[,] im1Points= new double[5,2] { {2071,876}, {1836,648}, {1607,1120}, {1384,1576}, {922,1351} };
    public double[,] im2Points= new double[5,2] { {1923,824}, {1667,589}, {1427,1093}, {1192,1583}, {721,1352} };
    public double[,] im3Points= new double[5,2] { {1962,834}, {1746,642}, {1578,1027}, {1398,1438}, {951,1223} };
    public double[,] hmMatrix1;
    public double[,] hmMatrix2;
    public double[,] hmMatrix3;
    public double[,] scenePointsTest= new double[3,2] { {0,0}, {100,100}, {300,0} };
    public double[,] im1PointsTest= new double[3,2] { {2071,642}, {1838,881}, {2072,1342} };
    public double[,] im2PointsTest= new double[3,2] { {1923,571}, {1670,837}, {1927,1325} };
    public double[,] im3PointsTest= new double[3,2] { {1944,645}, {1764,832}, {2002,1227} };

    // Start is called before the first frame update
    void Start()
    {
        //Calculating the homography matrix of the two point coordinates source and destination
        hmMatrix=calcHomography(source, destination);
        
        //Calculate estimated homographies
        hmMatrix1=calcHomography(scenePoints, im1Points);
        hmMatrix2=calcHomography(scenePoints, im2Points);
        hmMatrix3=calcHomography(scenePoints, im3Points);
        
    }

    public void operation1_1()
    {
        //Printing the calculated homography matrix
        print("Homography Matrix");
        printArrayD(hmMatrix);

        //Showing the calculated homography matrix is right
        print("\nTesting homography");
        print("x=293 y=868, u must be 3, v must be -6 after dividing with a scale");
        double[,] sourceM=new double[3,1];
        sourceM[0,0]=293;
        sourceM[1,0]=868;
        sourceM[2,0]=1;
        double[,] destinationM=applyProjection(sourceM, hmMatrix);
        print("Projection:");
        printArrayD(TransposeD(destinationM));
        print("\n");
    }

    public void operation1_2()
    {
        double[,] source = new double[4, 2] { { 93, 63 }, { 293, 868 }, { 1207, 998 }, { 1218, 309 } };
        double[,] destinationKarma = new double[4, 2] { { 3, -6 }, { 7, -4 }, { -7, 0 }, { 3, 2 } };
        double[,] degreeMatrix = new double[4,4] { {10,20,100,80}, {100, 80, 50, 70}, {10, 100, 25, 50}, {95, 90, 70, 100} };

        double[,] degreeMatrix2 = new double[4,4] { {55,70,90,85}, {10,95,75,90}, {98,88,85,80}, {97,85,90,70} };

        print("\nTesting homography with a mixed but fully matching pairs. Should give the same homography as 1.1");
        double[,] hmMatrixKarma=calcDegreedHomography(source, destinationKarma, degreeMatrix);
        printArrayD(hmMatrixKarma);

        print("\nTesting homography with a mixed non fully matching pairs. Best matches will be found and homography will be calculated");
        hmMatrixKarma=calcDegreedHomography(source, destinationKarma, degreeMatrix2);
        printArrayD(hmMatrixKarma);

        print("\n");
    }

    public void operation1_3()
    {
        double[,] point=new double[3,1];
        point[0,0]=1679;
        point[1,0]=128;
        point[2,0]=1;

        print("Doing projection over the point x="+point[0,0]+", y="+point[1,0]);
        double[,] projectionResult=applyProjection(point, hmMatrix);
        double[,] scaledResult=projection(point, hmMatrix);

        print("Result of u and v:");
        printArrayD(TransposeD(projectionResult));

        print("Result with scale:");
        printArrayD(TransposeD(scaledResult));
        
        print("\n");
    }

    public void operation1_4()
    {
        double[,] point=new double[3,1];
        point[0,0]=destination[0,0];
        point[1,0]=destination[0,1];
        point[2,0]=1;

        double[,] projectionResult=applyInvertedProjection(point, hmMatrix);
        print("Inverse projection of Points u="+destination[0,0]+", v="+destination[0,1]);
        printArrayD(TransposeD(projectionResult));
        print("\n");
    }

    public void operation1_5()
    {
        //***Im1***
        //Calculate estimated homography
        print("Estimated Homography Matrix for Image1");
        printArrayD(hmMatrix1);

        //Calculate projections of 3 points with estimated Homography
        double[,] point=new double[3,1];
        point[0,0]=scenePointsTest[0,0];
        point[1,0]=scenePointsTest[0,1];
        point[2,0]=1;
        double[,] estimatedProjection=applyProjection(point, hmMatrix1);
        print("Estimated Projection of point1 for image1:");
        printArrayD(TransposeD(estimatedProjection));
        print("Real Coordinates:");
        print(im1PointsTest[0,0]+", "+im1PointsTest[0,1]+", 1");
        double errors=calcError(estimatedProjection[0,0], estimatedProjection[1,0], im1PointsTest[0,0], im1PointsTest[0,1]);

        point[0,0]=scenePointsTest[1,0];
        point[1,0]=scenePointsTest[1,1];
        estimatedProjection=applyProjection(point, hmMatrix1);
        print("Estimated Projection of point2 for image1:");
        printArrayD(TransposeD(estimatedProjection));
        print("Real Coordinates:");
        print(im1PointsTest[1,0]+", "+im1PointsTest[1,1]+", 1");
        errors+=calcError(estimatedProjection[0,0], estimatedProjection[1,0], im1PointsTest[1,0], im1PointsTest[1,1]);

        point[0,0]=scenePointsTest[2,0];
        point[1,0]=scenePointsTest[2,1];
        estimatedProjection=applyProjection(point, hmMatrix1);
        print("Estimated Projection of point3 for image1:");
        printArrayD(TransposeD(estimatedProjection));
        print("Real Coordinates:");
        print(im1PointsTest[2,0]+", "+im1PointsTest[2,1]+", 1");
        errors+=calcError(estimatedProjection[0,0], estimatedProjection[1,0], im1PointsTest[2,0], im1PointsTest[2,1]);
        print("Error percentage for Image1:");
        print(errors/3);
        errors=0;

        print("\n");

        //***Im2***
        //Calculate estimated homography
        print("Estimated Homography Matrix for Image2");
        printArrayD(hmMatrix2);

        //Calculate projections of 3 points with estimated Homography
        point[0,0]=scenePointsTest[0,0];
        point[1,0]=scenePointsTest[0,1];
        estimatedProjection=applyProjection(point, hmMatrix2);
        print("Estimated Projection of point1 for image2:");
        printArrayD(TransposeD(estimatedProjection));
        print("Real Coordinates:");
        print(im2PointsTest[0,0]+", "+im2PointsTest[0,1]+", 1");
        errors=calcError(estimatedProjection[0,0], estimatedProjection[1,0], im2PointsTest[0,0], im2PointsTest[0,1]);

        point[0,0]=scenePointsTest[1,0];
        point[1,0]=scenePointsTest[1,1];
        estimatedProjection=applyProjection(point, hmMatrix2);
        print("Estimated Projection of point2 for image2:");
        printArrayD(TransposeD(estimatedProjection));
        print("Real Coordinates:");
        print(im2PointsTest[1,0]+", "+im2PointsTest[1,1]+", 1");
        errors+=calcError(estimatedProjection[0,0], estimatedProjection[1,0], im2PointsTest[1,0], im2PointsTest[1,1]);

        point[0,0]=scenePointsTest[2,0];
        point[1,0]=scenePointsTest[2,1];
        estimatedProjection=applyProjection(point, hmMatrix2);
        print("Estimated Projection of point3 for image2:");
        printArrayD(TransposeD(estimatedProjection));
        print("Real Coordinates:");
        print(im2PointsTest[2,0]+", "+im2PointsTest[2,1]+", 1");
        errors+=calcError(estimatedProjection[0,0], estimatedProjection[1,0], im2PointsTest[2,0], im2PointsTest[2,1]);
        print("Error percentage for Image2:");
        print(errors/3);
        errors=0;

        print("\n");

        //***Im3***
        //Calculate estimated homography
        print("Estimated Homography Matrix for Image2");
        printArrayD(hmMatrix3);

        //Calculate projections of 3 points with estimated Homography
        point[0,0]=scenePointsTest[0,0];
        point[1,0]=scenePointsTest[0,1];
        estimatedProjection=applyProjection(point, hmMatrix3);
        print("Estimated Projection of point1 for image3:");
        printArrayD(TransposeD(estimatedProjection));
        print("Real Coordinates:");
        print(im3PointsTest[0,0]+", "+im3PointsTest[0,1]+", 1");
        errors=calcError(estimatedProjection[0,0], estimatedProjection[1,0], im3PointsTest[0,0], im3PointsTest[0,1]);

        point[0,0]=scenePointsTest[1,0];
        point[1,0]=scenePointsTest[1,1];
        estimatedProjection=applyProjection(point, hmMatrix3);
        print("Estimated Projection of point2 for image3:");
        printArrayD(TransposeD(estimatedProjection));
        print("Real Coordinates:");
        print(im3PointsTest[1,0]+", "+im3PointsTest[1,1]+", 1");
        errors+=calcError(estimatedProjection[0,0], estimatedProjection[1,0], im3PointsTest[1,0], im3PointsTest[1,1]);

        point[0,0]=scenePointsTest[2,0];
        point[1,0]=scenePointsTest[2,1];
        estimatedProjection=applyProjection(point, hmMatrix3);
        print("Estimated Projection of point3 for image3:");
        printArrayD(TransposeD(estimatedProjection));
        print("Real Coordinates:");
        print(im3PointsTest[2,0]+", "+im3PointsTest[2,1]+", 1");
        errors+=calcError(estimatedProjection[0,0], estimatedProjection[1,0], im3PointsTest[2,0], im3PointsTest[2,1]);
        print("Error percentage for Image3:");
        print(errors/3);

        print("\n");
    }

    public void operation1_6()
    {
        double[,] s1=new double[3,1];
        double[,] s2=new double[3,1];
        double[,] s3=new double[3,1];

        s1[0,0]=75;
        s1[1,0]=55;
        s1[2,0]=1;
        s2[0,0]=63;
        s2[1,0]=33;
        s2[2,0]=1;
        s3[0,0]=1;
        s3[1,0]=1;
        s3[2,0]=1;

        //Im1
        double[,] projection=applyProjection(s1, hmMatrix1);
        print("Projection of s1 in image1:");
        printArrayD(TransposeD(projection));
        projection=applyProjection(s2, hmMatrix1);
        print("Projection of s2 in image1:");
        printArrayD(TransposeD(projection));
        projection=applyProjection(s3, hmMatrix1);
        print("Projection of s3 in image1:");
        printArrayD(TransposeD(projection));

        //Im2
        projection=applyProjection(s1, hmMatrix2);
        print("Projection of s1 in image2:");
        printArrayD(TransposeD(projection));
        projection=applyProjection(s2, hmMatrix2);
        print("Projection of s2 in image2:");
        printArrayD(TransposeD(projection));
        projection=applyProjection(s3, hmMatrix2);
        print("Projection of s3 in image2:");
        printArrayD(TransposeD(projection));

        //Im3
        projection=applyProjection(s1, hmMatrix3);
        print("Projection of s1 in image3:");
        printArrayD(TransposeD(projection));
        projection=applyProjection(s2, hmMatrix3);
        print("Projection of s2 in image3:");
        printArrayD(TransposeD(projection));
        projection=applyProjection(s3, hmMatrix3);
        print("Projection of s3 in image3:");
        printArrayD(TransposeD(projection));
    }

    public void operation1_7()
    {
        double[,] i1=new double[3,1];
        double[,] i2=new double[3,1];
        double[,] i3=new double[3,1];

        i1[0,0]=500;
        i1[1,0]=400;
        i1[2,0]=1;
        i2[0,0]=86;
        i2[1,0]=167;
        i2[2,0]=1;
        i3[0,0]=10;
        i3[1,0]=10;
        i3[2,0]=1;

        //Im1
        double[,] projection=applyInvertedProjection(i1, hmMatrix1);
        print("Projection of i1 in image1:");
        printArrayD(TransposeD(projection));
        projection=applyInvertedProjection(i2, hmMatrix1);
        print("Projection of i2 in image1:");
        printArrayD(TransposeD(projection));
        projection=applyInvertedProjection(i3, hmMatrix1);
        print("Projection of i3 in image1:");
        printArrayD(TransposeD(projection));

        //Im2
        projection=applyInvertedProjection(i1, hmMatrix2);
        print("Projection of i1 in image2:");
        printArrayD(TransposeD(projection));
        projection=applyInvertedProjection(i2, hmMatrix2);
        print("Projection of i2 in image2:");
        printArrayD(TransposeD(projection));
        projection=applyInvertedProjection(i3, hmMatrix2);
        print("Projection of i3 in image2:");
        printArrayD(TransposeD(projection));

        //Im3
        projection=applyInvertedProjection(i1, hmMatrix3);
        print("Projection of i1 in image3:");
        printArrayD(TransposeD(projection));
        projection=applyInvertedProjection(i2, hmMatrix3);
        print("Projection of i2 in image3:");
        printArrayD(TransposeD(projection));
        projection=applyInvertedProjection(i3, hmMatrix3);
        print("Projection of i3 in image3:");
        printArrayD(TransposeD(projection));
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

    public double[,] calcDegreedHomography(double[,] source, double[,] destination, double[,] degreeMatrix)
    {
        double[,] dMatrix=new double[destination.GetLength(0), 2];
        int[] order=new int[destination.GetLength(0)];
        double[,] resultHm=new double[3,3];
        double tempScore;
        double score=0;
        IList<int> orders=new List<int>();
        orders.Add(0);
        orders.Add(1);
        orders.Add(2);
        orders.Add(3);

        var collection=GetPermutations(orders, 4);
        foreach(var item in collection)
        {
            int i=0;
            tempScore=1;
            foreach(var number in item)
            {
                tempScore*=degreeMatrix[i,number];
                i++;
            }
            tempScore=tempScore/Math.Pow(100, i);


            if(tempScore>score)
            {
                score=tempScore;
                order=item.ToArray();
            }
        }
        print("Match score percentage: "+score*100);

        for(int i=0; i<destination.GetLength(0); ++i)
            for(int j=0; j<destination.GetLength(1); ++j)
                dMatrix[i,j]=destination[order[i], j];

        resultHm=calcHomography(source, dMatrix);

        return resultHm;
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

    public double[,] applyInvertedProjection(double[,] destinationM, double[,] hm)
    {
        //Invert the homography matrix
        double[,] invertedHmMatrix=invertMatrix(hm);

        double[,] projectionResult=projection(destinationM, invertedHmMatrix);
        for(int i=0; i<3; ++i)
            projectionResult[i,0]/=projectionResult[2,0];

        return projectionResult;
    }

    public double calcError(double p1x, double p1y, double p2x, double p2y)
    {
        double distance=Distance(p1x, p1y, p2x, p2y);
        double lengthOfVector=Math.Sqrt((p2x*p2x)+(p2y*p2y));

        return (100*distance)/lengthOfVector;
    }

    private double[,] invertMatrix(double[,] m)
    {
        double det = Determinant3DD(m);

        double invdet = 1 / det;

        double[,] minv=new double[3,3]; // inverse of matrix m
        minv[0, 0] = (m[1, 1] * m[2, 2] - m[2, 1] * m[1, 2]) * invdet;
        minv[0, 1] = (m[0, 2] * m[2, 1] - m[0, 1] * m[2, 2]) * invdet;
        minv[0, 2] = (m[0, 1] * m[1, 2] - m[0, 2] * m[1, 1]) * invdet;
        minv[1, 0] = (m[1, 2] * m[2, 0] - m[1, 0] * m[2, 2]) * invdet;
        minv[1, 1] = (m[0, 0] * m[2, 2] - m[0, 2] * m[2, 0]) * invdet;
        minv[1, 2] = (m[1, 0] * m[0, 2] - m[0, 0] * m[1, 2]) * invdet;
        minv[2, 0] = (m[1, 0] * m[2, 1] - m[2, 0] * m[1, 1]) * invdet;
        minv[2, 1] = (m[2, 0] * m[0, 1] - m[0, 0] * m[2, 1]) * invdet;
        minv[2, 2] = (m[0, 0] * m[1, 1] - m[1, 0] * m[0, 1]) * invdet;

        return minv;
    }

    static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
    {
        if (length == 1) return list.Select(t => new T[] { t });
        return GetPermutations(list, length - 1)
            .SelectMany(t => list.Where(o => !t.Contains(o)),
                (t1, t2) => t1.Concat(new T[] { t2 }));
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
