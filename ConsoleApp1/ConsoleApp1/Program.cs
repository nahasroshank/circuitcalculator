using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApp1
{
    public class MainClass
    {
        const int HORIZONTAL = 0;
        const int VERTICAL = 1;
        const int NEW_ENTRY = 2;
        const int OLD_ENTRY = 3;
        

        public class NetList
        {
            public string Label;
            public int Index;
            public int Node1, Node2;
            public double Value;
            public void PrintNetList()
            {
                Console.WriteLine("Label: {0}\nNode1: {1}\nNode2: {2}\nType: {3}\nValue: {4}\nIndex: {5}\n\n",
                        this.Label, this.Node1, this.Node2, this.getComponentType(), this.Value, this.Index);
            }
            public char getComponentType()
            {
                switch (Label[0])
                {
                    case 'R': return 'R';
                    case 'V': return 'V';
                    case 'C': return 'C';
                    default: return 'X';
                }
            }
            public void GetIndex(int arg)
            {
                this.Index = arg;
                Console.WriteLine("Index after updation in GetIndex {1}: {0}", Index, Label);
            }
            public void GetNetlist(String argType, int argNode1, int argNode2, int argValue)
            {
                Label = argType;
                Node1 = argNode1;
                Node2 = argNode2;
                Value = argValue;
            }
            public void GetNetlist(NetList obj)
            {
                Label = obj.Label;
                Node1 = obj.Node1;
                Node2 = obj.Node2;
                Value = obj.Value;
               
            }
        }
        public class NetListNode
        {
            public NetListNode next;
            public NetList data;
            public NetListNode()
            {
                next = null;
                data = new NetList();
            }
        }
        public class LinkedListNetList
        {
            public static int NetListCount = 0;
            public static int ResistorCount = 0;
            public static int VoltageSourceCount = 0;
            public static int CurrentSourceCount = 0;
            public NetListNode head, tail;
            public LinkedListNetList()
            {
                head = null;
                tail = null;
            }
            public void PrintLinkedListNetList()
            {
                NetListNode testNode = head;
                Console.WriteLine("List:\n");
                while (testNode != null)
                {
                    testNode.data.PrintNetList();
                    testNode = testNode.next;
                }
            }
            public void AddNode(ref NetList argData,int mode)
            {
                Console.WriteLine("Adding node {0}",argData.Label);
                Console.WriteLine("Type : {0}",argData.getComponentType());
                NetListNode toAdd = new NetListNode();
                toAdd.data.GetNetlist(argData);
                if (mode == NEW_ENTRY)
                {
                    NetListCount++;
                }
                switch (argData.getComponentType())
                {
                    case 'R':
                    if (mode == NEW_ENTRY) ++ResistorCount;
                        Console.WriteLine("Index of {0} before GetIndex(): {1}", toAdd.data.Label, toAdd.data.Index);
                        toAdd.data.GetIndex(ResistorCount);
                        Console.WriteLine("Index of {0} after GetIndex(): {1}", toAdd.data.Label, toAdd.data.Index);
                    break;
                    case 'V':
                        if(mode == NEW_ENTRY) ++VoltageSourceCount;
                        Console.WriteLine("Index of {0} before GetIndex(): {1}", toAdd.data.Label, toAdd.data.Index);
                        toAdd.data.GetIndex(VoltageSourceCount);
                        Console.WriteLine("Index of {0} after GetIndex(): {1}", toAdd.data.Label, toAdd.data.Index);
                    break;
                    case 'C':
                        if(mode == NEW_ENTRY) ++CurrentSourceCount;
                        Console.WriteLine("Index of {0} before GetIndex(): {1}", toAdd.data.Label, toAdd.data.Index);
                        toAdd.data.GetIndex(CurrentSourceCount);
                        Console.WriteLine("Index of {0} after GetIndex(): {1}", toAdd.data.Label, toAdd.data.Index);
                    break;
                }
                if (head == null)
                {
                    head = toAdd;
                    tail = toAdd;
                }
                else
                {
                    tail.next = toAdd;
                    tail = toAdd;
                }
            }
            public int NodeCount()
            {
                NetListNode testNode = this.head;
                int nodeNum = 0;
                while (testNode != null)
                {
                    if (testNode.data.Node1 > nodeNum) nodeNum = testNode.data.Node1;
                    if (testNode.data.Node2 > nodeNum) nodeNum = testNode.data.Node2;
                    testNode = testNode.next;
                }
                return nodeNum;
            }
            public int VoltageCount()
            {
                NetListNode testElement = this.head;
                int voltageNum = 0;
                while (testElement != null)
                {
                    //need input format
                    voltageNum++;
                    testElement = testElement.next;
                    Console.WriteLine("Counting elements for voltage source count...");
                }
                return voltageNum;
            }
        }
        public class Program
        {
            const int HORIZONTAL = 0;
            const int VERTICAL = 1;
            const int NEW_ENTRY = 2;
            const int OLD_ENTRY = 3;

            static public double[] getRow(double[,] argMatrix, int row)
            {
                double[] returnRow = new double[argMatrix.GetLength(1)];
                for (int i = 0; i < argMatrix.GetLength(1); i++)
                    returnRow[i] = argMatrix[row, i];
                return returnRow;
            }
            static public void CopyRow(double[,] argMatrix, int argRow, ref double[,] destMatrix, int destRow)
            {
                for (int i = 0; i < argMatrix.GetLength(1); i++)
                    destMatrix[destRow, i] = argMatrix[argRow, i];
            }
            static public void CopyRow(double[] argRow, ref double[,] destMatrix, int destRow)
            {
                for (int i = 0; i < argRow.Length; i++)
                    destMatrix[destRow, i] = argRow[i];
            }
            static public double[,] GetMatrixG(int row, int col, LinkedListNetList argList)
            {
                Console.WriteLine("MatrixG argList:");
                argList.PrintLinkedListNetList();
                double[,] returnMatrix = new double[row, col]; // default elements set to zero
                NetListNode testNode = argList.head;
                while (testNode != null)
                {
                    if (testNode.data.getComponentType() == 'R')
                    {   // minus one since Matrix index starts from zero
                        Console.WriteLine("Resistor found in MatG arglist");
                        if (testNode.data.Node1 > 0)
                        {
                            // Console.WriteLine("Value of conductance: {0}", 1 / testNode.data.Value);
                            // Console.WriteLine("Value of resistance: {0}", testNode.data.Value);
                            returnMatrix[testNode.data.Node1 - 1, testNode.data.Node1 - 1] = returnMatrix[testNode.data.Node1 - 1, testNode.data.Node1 - 1] + (1 / testNode.data.Value);

                        }
                        if (testNode.data.Node2 > 0)
                        {
                            // Console.WriteLine("Value of conductance: {0}", 1 / testNode.data.Value);
                            // Console.WriteLine("Value of resistance: {0}", testNode.data.Value);
                            returnMatrix[testNode.data.Node2 - 1, testNode.data.Node2 - 1] = returnMatrix[testNode.data.Node2 - 1, testNode.data.Node2 - 1] + (1 / testNode.data.Value);

                        }
                        if (testNode.data.Node1 > 0 && testNode.data.Node2 > 0)
                        {
                            // Console.WriteLine("Nodes not zero");
                            // Console.WriteLine("Value of conductance: {0}", 1 / testNode.data.Value);
                            // Console.WriteLine("Value of resistance: {0}", testNode.data.Value);
                            returnMatrix[testNode.data.Node1 - 1, testNode.data.Node2 - 1] -= (1 / testNode.data.Value);
                            returnMatrix[testNode.data.Node2 - 1, testNode.data.Node1 - 1] -= (1 / testNode.data.Value);
                        }
                    }
                    testNode = testNode.next;
                }
                return returnMatrix;
            }
            static public double[,] GetMatrixB(int row, int col, LinkedListNetList argList)
            {
                Console.WriteLine("MatrixB argList:");
                argList.PrintLinkedListNetList();
                double[,] returnMatrix = new double[row, col]; // default elements set to zero
                NetListNode testNode = argList.head;
                while (testNode != null)
                {
                    if (testNode.data.getComponentType() == 'V')
                    {   // minus one since Matrix index starts from zero
                        Console.WriteLine("Voltage found in MatB arglist");
                        testNode.data.PrintNetList();
                        if (testNode.data.Node1 != 0) returnMatrix[testNode.data.Node1 - 1, testNode.data.Index - 1] = 1;
                        else returnMatrix[testNode.data.Node1 - 1, testNode.data.Index - 1] = 0;
                        if (testNode.data.Node2 != 0) returnMatrix[testNode.data.Node2 - 1, testNode.data.Index - 1] = -1;
                        else returnMatrix[testNode.data.Node1 - 1, testNode.data.Index - 1] = 0;
                    }
                    testNode = testNode.next;
                }
                return returnMatrix;
            }
            static public double[,] GetMatrixI(int row, int col, LinkedListNetList argList)
            {
                double[,] returnMatrix = new double[row, col]; // default elements set to zero
                NetListNode testNode = argList.head;
                while (testNode != null)
                {
                    // minus one since Matrix index starts from zero
                    if (testNode.data.Node2 > 0)
                    {
                        returnMatrix[testNode.data.Node2 - 1, 1] += testNode.data.Value;
                    }
                    testNode = testNode.next;
                }
                return returnMatrix;
            }
            static public double[,] GetMatrixE(int row, int col, LinkedListNetList argList)
            {
                double[,] returnMatrix = new double[row, col]; // default elements set to zero
                NetListNode testNode = argList.head;
                while (testNode != null)
                {
                    // minus one since Matrix index starts from zero
                    returnMatrix[testNode.data.Index - 1, 1] = testNode.data.Value;
                    testNode = testNode.next;
                }
                return returnMatrix;
            }
            static public double[,] TransposeMatrix(int row, int col, double[,] argMatrix)
            {
                double[,] returnMatrix = new double[col, row]; // swap dimensions
                for (int i = 0; i < col; i++)
                    for (int j = 0; j < row; j++)
                        returnMatrix[i, j] = argMatrix[j, i];
                return returnMatrix;
            }
            static public double[,] ConcatenateMatrix(double[,] argMatrix1, double[,] argMatrix2, int mode)
            {
                int Mat1Len = argMatrix1.GetLength(0);
                int Mat1Bre = argMatrix1.GetLength(1);
                int Mat2Len = argMatrix2.GetLength(0);
                int Mat2Bre = argMatrix2.GetLength(1);
                double[,] returnMatrix = new double[Mat1Len, Mat2Len + Mat2Bre];
                for (int i = 0; i < Mat1Len; i++)
                    for (int j = 0; j < Mat1Bre; j++)
                        returnMatrix[i, j] = argMatrix1[i, j];
                if (mode == HORIZONTAL)
                {
                    for (int i = 0; i < Mat1Len; i++)
                        for (int j = Mat1Bre; j < Mat1Bre + Mat2Bre; j++)
                            returnMatrix[i, j] = argMatrix2[i, j - Mat1Bre];
                }
                if (mode == VERTICAL)
                {
                    for (int i = Mat1Len; i < Mat1Len + Mat2Len; i++)
                        for (int j = 0; j < Mat1Bre; j++)
                            returnMatrix[i, j] = argMatrix2[i - Mat1Len, j];
                }
                return returnMatrix;
            }
            static public double[,] DuplicateMatrix(double[,] argMatrix)
            {
                double[,] returnMatrix = new double[argMatrix.GetLength(0), argMatrix.GetLength(1)];
                for (int i = 0; i < argMatrix.GetLength(0); i++)
                    for (int j = 0; j < argMatrix.GetLength(1); i++)
                        returnMatrix[i, j] = argMatrix[i, j];
                return returnMatrix;
            }
            static public double[,] DecomposeMatrix(double[,] argMatrix, out int[] perm, out int toggle)
            {
                int rows = argMatrix.Length;
                int cols = argMatrix.Length; // assume square
                if (rows != cols)
                    throw new Exception("Attempt to decompose a non-square m");

                int n = rows; // convenience

                double[,] result = DuplicateMatrix(argMatrix);

                perm = new int[n]; // set up row permutation result
                for (int i = 0; i < n; ++i) { perm[i] = i; }

                toggle = 1; // toggle tracks row swaps.
                            // +1 -greater-than even, -1 -greater-than odd. used by MatrixDeterminant

                for (int j = 0; j < n - 1; ++j) // each column
                {
                    double colMax = Math.Abs(result[j, j]); // find largest val in col
                    int pRow = j;
                    for (int i = j + 1; i < n; ++i)
                    {
                        if (Math.Abs(result[i, j]) > colMax)
                        {
                            colMax = Math.Abs(result[i, j]);
                            pRow = i;
                        }
                    }
                    if (pRow != j) // if largest value not on pivot, swap rows
                    {
                        double[] rowPtr = getRow(result, pRow);
                        CopyRow(result, j, ref result, pRow);
                        CopyRow(rowPtr, ref result, j);
                        // result[j] = rowPtr;

                        int tmp = perm[pRow]; // and swap perm info
                        perm[pRow] = perm[j];
                        perm[j] = tmp;

                        toggle = -toggle; // adjust the row-swap toggle
                    }
                    if (result[j, j] == 0.0)
                    {
                        // find a good row to swap
                        int goodRow = -1;
                        for (int row = j + 1; row < n; ++row)
                        {
                            if (result[row, j] != 0.0)
                                goodRow = row;
                        }
                        if (goodRow == -1)
                            throw new Exception("Cannot use Doolittle's method");
                        // swap rows so 0.0 no longer on diagonal
                        double[] rowPtr = getRow(result, goodRow);
                        CopyRow(result, j, ref result, goodRow);
                        // result[goodRow] = result[j];
                        CopyRow(rowPtr, ref result, j);
                        // result[j] = rowPtr;

                        int tmp = perm[goodRow]; // and swap perm info
                        perm[goodRow] = perm[j];
                        perm[j] = tmp;

                        toggle = -toggle; // adjust the row-swap toggle
                    }
                    // --------------------------------------------------
                    // if diagonal after swap is zero . .
                    //if (Math.Abs(result[j][j]) less-than 1.0E-20) 
                    //  return null; // consider a throw

                    for (int i = j + 1; i < n; ++i)
                    {
                        result[i, j] /= result[j, j];
                        for (int k = j + 1; k < n; ++k)
                            result[i, k] -= result[i, j] * result[j, k];
                    }
                } // main j column loop
                return result;
            }
            static public double[] HelperSolve(double[,] luMatrix, double[] b)
            {
                // before calling this helper, permute b using the perm array
                // from MatrixDecompose that generated luMatrix
                int n = luMatrix.Length;
                double[] x = new double[n];
                b.CopyTo(x, 0);
                for (int i = 1; i < n; ++i)
                {
                    double sum = x[i];
                    for (int j = 0; j < i; ++j)
                        sum -= luMatrix[i, j] * x[j];
                    x[i] = sum;
                }
                x[n - 1] /= luMatrix[n - 1, n - 1];
                for (int i = n - 2; i >= 0; --i)
                {
                    double sum = x[i];
                    for (int j = i + 1; j < n; ++j)
                        sum -= luMatrix[i, j] * x[j];
                    x[i] = sum / luMatrix[i, i];
                }
                return x;
            }
            static public double[,] InverseMatrix(double[,] argMatrix)
            {
                int n = argMatrix.Length;
                double[,] result = DuplicateMatrix(argMatrix);
                int[] perm;
                int toggle;
                double[,] lum = DecomposeMatrix(argMatrix, out perm, out toggle);
                if (lum == null)
                    throw new Exception("Unable to compute inverse");
                double[] b = new double[n];
                for (int i = 0; i < n; ++i)
                {
                    for (int j = 0; j < n; ++j)
                    {
                        if (i == perm[j]) b[j] = 1.0;
                        else b[j] = 0.0;
                    }
                    double[] x = HelperSolve(lum, b); // 
                    for (int j = 0; j < n; ++j)
                        result[j, i] = x[j];
                }
                return result;
            }
            static public double[,] ProductMatrix(double[,] argMatrix1, double[,] argMatrix2)
            {
                int Row1 = argMatrix1.GetLength(0);
                int Col1 = argMatrix1.GetLength(1);
                int Row2 = argMatrix2.GetLength(0);
                int Col2 = argMatrix2.GetLength(1);
                if (Col1 != Row2)
                    throw new Exception("Non-conformable matrices in MatrixProduct");

                double[,] result = new double[Row1, Col2];

                for (int i = 0; i < Row1; ++i) // each row of A
                    for (int j = 0; j < Col2; ++j) // each col of B
                        for (int k = 0; k < Col1; ++k) // could use k less-than bRows
                            result[i, j] += argMatrix1[i, k] * argMatrix2[k, j];
                return result;
            }
            static public void PrintMatrix(double [,] argMatrix)
            {
                for(int i = 0; i < argMatrix.GetLength(0); i++)
                {
                    for(int j = 0; j < argMatrix.GetLength(1); j++)
                        Console.Write("{0:E2}\t",argMatrix[i, j]);
                    Console.Write("\n");
                }
            }

            public static void Main(string[] args)
            {
                LinkedListNetList NetListObj = new LinkedListNetList();
                LinkedListNetList ResistorList = new LinkedListNetList();
                LinkedListNetList VoltageSourceList = new LinkedListNetList();
                LinkedListNetList CurrentSourceList = new LinkedListNetList();

                // Console.Write("Number of elements: ");
                // string NetListNumString = Console.ReadLine();
                // int NetListNum = Convert.ToInt32(NetListNumString);
                int NetListNum = 5;
                string[] NetListStringArray = new string[]{"V1 1 0 15", "R1 1 2 2700", "R2 2 0 5000", "R35 2 3 1000000", "RZ 3 0 10000"};
                for (int i = 0; i < NetListNum; i++)
                {
                    // NetListStringArray[i] = Console.ReadLine();
                    // for (int i = 0; i < args.Length; i++) // extracting NetList from input and storing in linked list
                    string[] parts = NetListStringArray[i].Split(' ');
                    string Type = parts[0];
                    string Node1str = parts[1];
                    string Node2str = parts[2];
                    string Valuestr = parts[3];
                    int Node1 = Convert.ToInt32(Node1str);
                    int Node2 = Convert.ToInt32(Node2str);
                    int Value = Convert.ToInt32(Valuestr);
                    NetList newElement = new NetList();
                    newElement.GetNetlist(Type, Node1, Node2, Value);
                    NetListObj.AddNode(ref newElement, NEW_ENTRY);
                    switch (newElement.getComponentType())
                    {
                        case 'R':
                            Console.WriteLine("newElement read resistor");
                            ResistorList.AddNode(ref newElement,OLD_ENTRY);
                            break;
                        case 'V':
                            Console.WriteLine("newElement read voltage");
                            VoltageSourceList.AddNode(ref newElement,OLD_ENTRY);
                            break;
                        case 'C':
                            Console.WriteLine("newElement read current");
                            CurrentSourceList.AddNode(ref newElement,OLD_ENTRY);
                            break;
                        default:
                            //
                            break;

                    }
                }
                Console.WriteLine("Read all netlist...");
                NetListObj.PrintLinkedListNetList();
                VoltageSourceList.PrintLinkedListNetList();
                ResistorList.PrintLinkedListNetList();
                // asdasd
                int nodeNum = NetListObj.NodeCount(); // counts non-redundant nodes
                int voltageNum = VoltageSourceList.VoltageCount(); // counts individual voltage nodes from list
                Console.WriteLine("{0} {1}", nodeNum, voltageNum);
                double[,] matrixA = new double[nodeNum + voltageNum, nodeNum + voltageNum];
                double[,] matrixG = new double[nodeNum, nodeNum];
                double[,] matrixB = new double[nodeNum, voltageNum];
                double[,] matrixC = new double[voltageNum, nodeNum];
                double[,] matrixD = new double[voltageNum, voltageNum]; // matrixD is zero, no dependent sources assumed
                double[,] matrixX = new double[nodeNum + voltageNum, 1]; // unknown
                double[,] matrixZ = new double[nodeNum + voltageNum, 1];
                double[,] matrixI = new double[nodeNum, 1];
                double[,] matrixE = new double[voltageNum, 1];
                double[,] matrixAsub1 = new double[nodeNum, nodeNum + voltageNum];
                double[,] matrixAsub2 = new double[voltageNum, nodeNum + voltageNum];
                double[,] matrixAinv = new double[nodeNum + voltageNum, nodeNum + voltageNum];
                Console.WriteLine("Init matrix done");
                matrixG = GetMatrixG(nodeNum, nodeNum, NetListObj);
                Console.WriteLine("MatrixG done");
                PrintMatrix(matrixG);
                matrixB = GetMatrixB(nodeNum, voltageNum, NetListObj);
                Console.WriteLine("MatrixB done");
                PrintMatrix(matrixB);
                matrixC = TransposeMatrix(nodeNum, voltageNum, matrixB);
                Console.WriteLine("MatrixC done");
                matrixI = GetMatrixI(nodeNum, 1, CurrentSourceList);
                Console.WriteLine("MatrixI done");
                matrixAsub1 = ConcatenateMatrix(matrixG, matrixB, HORIZONTAL);
                Console.WriteLine("MatrixAsub1 done");
                matrixAsub2 = ConcatenateMatrix(matrixC, matrixD, HORIZONTAL);
                Console.WriteLine("MatrixAsub2 done");
                matrixA = ConcatenateMatrix(matrixAsub1, matrixAsub2, VERTICAL); // matrixA done
                Console.WriteLine("MatrixA done");
                PrintMatrix(matrixA);
                matrixZ = ConcatenateMatrix(matrixI, matrixE, VERTICAL); // matrixZ done
                matrixAinv = InverseMatrix(matrixA);
                matrixX = ProductMatrix(matrixAinv, matrixZ);
                Console.WriteLine("MatrixX");
            }
        }
    }
}

           
    

    

