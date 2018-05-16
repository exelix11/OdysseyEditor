using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Diagnostics;
using BnTxx.Formats;

namespace Smash_Forge
{
    public class Vector2
    {
        public float X, Y;
    }

    public class Vector3
    {
        public float X, Y, Z;
        public Vector3(float x, float y, float z)
        {
            X = x; Y = y; Z = z;
        }

        public Vector3() { }
    }

    public class Vector4
    {
        public float X, Y, Z , W;
        public Vector4(float x, float y, float z,float w)
        {
            X = x; Y = y; Z = z; W = w;
        }
        public Vector4() { }
    }

    public class BFRES
    {
        public List<string> stringContainer = new List<string>();
        public List<FMDL_Model> models = new List<FMDL_Model>();
        public Dictionary<string, BinaryTexture> textures = new Dictionary<string, BinaryTexture>();
        
        public static int sign10Bit(int i)
        {
            if (((i >> 9) & 0x1) == 1)
            {
                i = ~i;
                i = i & 0x3FF;
                i += 1;
                i *= -1;
            }

            return i;
        }

        public int readOffset(FileData f)
        {
            return f.pos() + f.readInt();
        }        
        
        public static int verNumA, verNumB, verNumC, verNumD, SwitchCheck;

        public void Read(string filename) => Read(File.ReadAllBytes(filename));

        public void Read(byte[] file)
        {
            FileData f = new FileData(file);
            f.Endian = Endianness.Big;
            f.seek(0);

            f.seek(4); // magic check
            SwitchCheck = f.readInt(); //Switch version only has padded magic
            verNumD = f.readByte();
            verNumC = f.readByte();
            verNumB = f.readByte();
            verNumA = f.readByte();
            if (SwitchCheck == 0x20202020)
            {
                //Console.WriteLine("Version = " + verNumA + "." + verNumB + "." + verNumC + "." + verNumD);
                if (f.readShort() == 0xFEFF)
                    f.Endian = Endianness.Big;
                else f.Endian = Endianness.Little;
                f.skip(2);  //Size Headeer
                f.skip(4); //File Name Direct
                int fileAlignment = f.readInt();
                int RelocationTableOffset = f.readInt();
                int BfresSize = f.readInt();

                string name = f.readString(readOffset(f) + 2, -1);

                f.skip(4); // Padding
                long FMDLOffset = f.readInt64();
                long FMDLDict = f.readInt64();
                long FSKAOffset = f.readInt64();
                long FSKADict = f.readInt64();
                long FMAAOffset = f.readInt64();
                long FMAADict = f.readInt64();
                long FVISOffset = f.readInt64();
                long FVISDict = f.readInt64();
                long FSHUOffset = f.readInt64();
                long FSHUDict = f.readInt64();
                long FSCNOffset = f.readInt64();
                long FSCNDict = f.readInt64();
                long BuffMemPool = f.readInt64();
                long BuffMemPoolInfo = f.readInt64();
                long EMBOffset = f.readInt64();
                long EMBDict = f.readInt64();
                f.skip(8); // Padding
                long StringTableOffset = f.readInt64();
                int unk11 = f.readInt();
                int FMDLCount = f.readShort();
                /*FSKACount =*/ f.readShort();
                int FMAACount = f.readShort();
                int FVISCount = f.readShort();
                int FSHUCount = f.readShort();
                int FSCNCount = f.readShort();
                int EMBCount = f.readShort();
                f.skip(12); // Padding
                            // //Console.WriteLine($"FMDLOffset {FMDLOffset} FMDLCount {FMDLCount} FMDLDict {FMDLDict} FSKAOffset {FSKAOffset} FSKADict {FSKADict}");
                            //  //Console.WriteLine($"FMAAOffset {FMAAOffset} FMAADict {FMAADict} FVISOffset {FVISOffset} FSHUOffset {FSKAOffset} FSKADict {FSHUDict}");

                //FMDLs -Models-
                for (int i = 0; i < EMBCount; i++)
                {
                    f.seek((int)EMBOffset + (i * 16));
                    int DataOffset = f.readInt();
                    f.seek(DataOffset);
                    string EmMagic = f.readString(f.pos(), 4);

                    if (EmMagic.Equals("BNTX")) //Textures
                    {
                        f.skip(24);
                        int size = f.readInt();
                        f.seek(DataOffset);
                        BinaryTexture t = new BinaryTexture(f.GetStream(size));
                        textures.Add(t.Name, t);
                    }
                }
                f.seek((int)FMDLOffset);
                for (int i = 0; i < FMDLCount; i++)
                {
                    //   //Console.WriteLine("Reading FMDL....");

                    FMDL_Model model = new FMDL_Model();



                    //FMDL modelTest = new FMDL();
                    //modelTest.Read(f);
                    f.skip(16);

                    FMDLheader fmdl_info = new FMDLheader
                    {

                        name = f.readString(f.readInt() + 2, -1),
                        padding = f.readInt(),
                        eofString = f.readInt64(),
                        fsklOff = f.readInt64(),
                        fvtxArrOff = f.readInt64(),
                        fshpOffset = f.readInt64(),
                        fshpIndx = f.readInt64(),
                        fmatOffset = f.readInt64(),
                        fmatIndx = f.readInt64(),
                        UserDataOffset = f.readInt64(),
                        padding1 = f.readInt64(),
                        padding2 = f.readInt64(),
                        fvtxCount = f.readShort(),
                        fshpCount = f.readShort(),
                        fmatCount = f.readShort(),
                        paramCount = f.readShort(),
                        VertCount = f.readInt(),
                        unk2 = f.readInt(),
                    };
                    int NextFMDL = f.pos();

                    //Models.Nodes.Add(fmdl_info.name);
                    //   //Console.WriteLine($" Name {fmdl_info.name} eofString {fmdl_info.eofString} fsklOff {fmdl_info.fsklOff}");
                    //  //Console.WriteLine(fmdl_info.fvtxCount);

                    List<FVTXH> FVTXArr = new List<FVTXH>();
                    f.seek((int)fmdl_info.fvtxArrOff);
                    for (int vtx = 0; vtx < fmdl_info.fvtxCount; vtx++)
                    {
                        //   //Console.WriteLine("Reading FVTX....");
                        f.skip(16);
                        FVTXArr.Add(new FVTXH
                        {
                            attArrOff = f.readInt64(),
                            attIndxOff = f.readInt64(),
                            unk1 = f.readInt64(),
                            unk2 = f.readInt64(),
                            unk3 = f.readInt64(),
                            buffSizeOff = f.readInt64(),
                            buffStrideSizeOff = f.readInt64(),
                            buffArrOff = f.readInt64(),
                            buffOff = f.readInt(),
                            attCount = f.readByte(),
                            buffCount = f.readByte(),
                            sectIndx = f.readShort(),
                            vertCount = f.readInt(),
                            SkinWeightInfluence = f.readInt()
                        });
                        //  //Console.WriteLine($"attCount {FVTXArr[vtx].attCount}");
                    }


                    f.seek((int)fmdl_info.fmatOffset);
                    List<FMATH> FMATheaders = new List<FMATH>();
                    for (int mat = 0; mat < fmdl_info.fmatCount; mat++)
                    {
                        //    //Console.WriteLine("Reading FMAT....");
                        f.skip(16);


                        FMATH fmat_info = new FMATH
                        {
                            name = f.readString((int)f.readInt64() + 2, -1),
                            renderInfoOff = f.readInt64(),
                            renderInfoIndx = f.readInt64(),
                            shaderAssignOff = f.readInt64(),
                            u1 = f.readInt64(),
                            texSelOff = f.readInt64(),
                            u2 = f.readInt64(),
                            texAttSelOff = f.readInt64(),
                            texAttIndxOff = f.readInt64(),
                            matParamArrOff = f.readInt64(),
                            matParamIndxOff = f.readInt64(),
                            matParamOff = f.readInt64(),
                            userDataOff = f.readInt64(),
                            userDataIndxOff = f.readInt64(),
                            volatileFlagOffset = f.readInt64(),
                            u3 = f.readInt64(),
                            samplerSlotOff = f.readInt64(),
                            textureSlotOff = f.readInt64(),
                            flags = f.readInt(), //This toggles material visabilty
                            sectIndx = f.readShort(),
                            rendParamCount = f.readShort(),
                            texSelCount = f.readByte(),
                            texAttSelCount = f.readByte(),
                            matParamCount = f.readShort(),
                            u4 = f.readShort(),
                            matParamSize = f.readShort(),
                            rawParamDataSize = f.readShort(),
                            userDataCount = f.readShort(),
                            padding = f.readInt(),

                        };
                        string FMATNameOffset = fmat_info.name;
                        // //Console.WriteLine($"{FMATNameOffset} {fmat_info.texSelCount} ");
                        FMATheaders.Add(fmat_info);
                    }

                    f.seek((int)fmdl_info.fsklOff + 16);
                    // //Console.WriteLine("Reading FSKL....");
                    FSKLH fskl_info = new FSKLH
                    {
                        boneIndxOff = f.readInt64(),
                        boneArrOff = f.readInt64(),
                        invIndxArrOff = f.readInt64(),
                        invMatrArrOff = f.readInt64(),
                        padding1 = f.readInt64(),
                        fsklType = f.readInt(), //flags
                        boneArrCount = f.readShort(),
                        invIndxArrCount = f.readShort(),
                        exIndxCount = f.readShort(),
                        u1 = f.readInt(),
                    };

                    f.seek((int)fmdl_info.fsklOff + 16);
                    FSKLH fskl_infov8 = new FSKLH
                    {
                        boneIndxOff = f.readInt64(),
                        boneArrOff = f.readInt64(),
                        invIndxArrOff = f.readInt64(),
                        invMatrArrOff = f.readInt64(),
                        padding1 = f.readInt64(),
                        padding2 = f.readInt64(),
                        padding3 = f.readInt64(),
                        fsklType = f.readInt(), //flags
                        boneArrCount = f.readShort(),
                        invIndxArrCount = f.readShort(),
                        exIndxCount = f.readShort(),
                        u1 = f.readInt(),
                    };
                    //  //Console.WriteLine($"Bone Count {fskl_info.boneArrCount}");

                    //FSKL and many other sections will be revised and cleaner later

                    if (verNumB == 8)
                    {
                        model.Node_Array = new int[fskl_infov8.invIndxArrCount + fskl_infov8.exIndxCount];
                        f.seek((int)fskl_infov8.invIndxArrOff);
                        for (int nodes = 0; nodes < fskl_infov8.invIndxArrCount + fskl_infov8.exIndxCount; nodes++)
                        {
                            model.Node_Array[nodes] = (f.readShort());
                        }
                    }
                    else
                    {
                        model.Node_Array = new int[fskl_info.invIndxArrCount + fskl_info.exIndxCount];
                        f.seek((int)fskl_info.invIndxArrOff);
                        for (int nodes = 0; nodes < fskl_info.invIndxArrCount + fskl_info.exIndxCount; nodes++)
                        {
                            model.Node_Array[nodes] = (f.readShort());
                        }
                    }



                    List<FSHPH> FSHPArr = new List<FSHPH>();
                    // //Console.WriteLine("Reading FSHP....");
                    f.seek((int)fmdl_info.fshpOffset);
                    for (int shp = 0; shp < fmdl_info.fshpCount; shp++)
                    {
                        f.skip(16);
                        FSHPArr.Add(new FSHPH
                        {
                            polyNameOff = f.readInt(),
                            u1 = f.readInt(),
                            fvtxOff = f.readInt64(),
                            lodMdlOff = f.readInt64(),
                            fsklIndxArrOff = f.readInt64(),
                            u3 = f.readInt64(),
                            u4 = f.readInt64(),
                            boundingBoxOff = f.readInt64(),
                            radiusOff = f.readInt64(),
                            padding = f.readInt64(),
                            flags = f.readInt(),
                            sectIndx = f.readShort(),
                            fmatIndx = f.readShort(),
                            fsklIndx = f.readShort(),
                            fvtxIndx = f.readShort(),
                            fsklIndxArrCount = f.readShort(),
                            matrFlag = f.readByte(),
                            lodMdlCount = f.readByte(),
                            visGrpCount = f.readInt(),
                            visGrpIndxOff = f.readShort(),
                            visGrpNodeOff = f.readShort(),
                        });
                    }

                    // //Console.WriteLine("Reading Bones....");
                   
                    // //Console.WriteLine("Reading FSHP Array....");

                    //MeshTime!!

                    for (int m = 0; m < FSHPArr.Count; m++)
                    {

                        Mesh poly = new Mesh();


                        poly.name = f.readString(FSHPArr[m].polyNameOff + 2, -1);


                        //    //Console.WriteLine("Polygon = " + poly.name);

                        List<attdata> AttrArr = new List<attdata>();
                        f.seek((int)FVTXArr[FSHPArr[m].fvtxIndx].attArrOff);
                        for (int att = 0; att < FVTXArr[FSHPArr[m].fvtxIndx].attCount; att++)
                        {
                            string AttType = f.readString(f.readInt() + 2, -1);

                            f.skip(4); //padding  
                            f.Endian = Endianness.Big;
                            int vertType = f.readShort();
                            f.skip(2);
                            f.Endian = Endianness.Little;
                            int buffOff = f.readShort();
                            int buffIndx = f.readShort();
                            //   //Console.WriteLine($"{AttType} Type = {vertType} Offset = {buffOff} Index = {buffIndx} ");
                            AttrArr.Add(new attdata { attName = AttType, buffIndx = buffIndx, buffOff = buffOff, vertType = vertType });
                        }


                        //Get RLT real quick for buffer offset
                        f.seek(0x18);
                        int RTLOffset = f.readInt();

                        f.seek(RTLOffset);
                        f.skip(0x030);
                        int DataStart = f.readInt();

                        // //Console.WriteLine($"RLT {DataStart}");


                        List<buffData> BuffArr = new List<buffData>();
                        f.seek((int)FVTXArr[FSHPArr[m].fvtxIndx].buffArrOff);
                        for (int buff = 0; buff < FVTXArr[FSHPArr[m].fvtxIndx].buffCount; buff++)
                        {
                            buffData data = new buffData();
                            f.seek((int)FVTXArr[FSHPArr[m].fvtxIndx].buffSizeOff + ((buff) * 0x10));
                            data.buffSize = f.readInt();
                            f.seek((int)FVTXArr[FSHPArr[m].fvtxIndx].buffStrideSizeOff + ((buff) * 0x10));
                            data.strideSize = f.readInt();

                            //So these work by grabbing the RLT offset first and then adding the buffer offset. Then they keep adding each other by their buffer sizes
                            if (buff == 0) data.DataOffset = (DataStart + FVTXArr[FSHPArr[m].fvtxIndx].buffOff);
                            if (buff > 0) data.DataOffset = BuffArr[buff - 1].DataOffset + BuffArr[buff - 1].buffSize;
                            if (data.DataOffset % 8 != 0) data.DataOffset = data.DataOffset + (8 - (data.DataOffset % 8));

                            BuffArr.Add(data);
                            //   //Console.WriteLine("Data Offset = " + data.DataOffset + " Vertex Buffer Size =" + data.buffSize + " Index = " + buff + " vertexStrideSize size =" + data.strideSize);
                        }

                        for (int v = 0; v < FVTXArr[FSHPArr[m].fvtxIndx].vertCount; v++)
                        {
                            Vertex vert = new Vertex();
                            for (int attr = 0; attr < AttrArr.Count; attr++)
                            {
                                f.seek(((BuffArr[AttrArr[attr].buffIndx].DataOffset) + (AttrArr[attr].buffOff) + (BuffArr[AttrArr[attr].buffIndx].strideSize * v)));
                                switch (AttrArr[attr].attName)
                                {
                                    case "_p0":
                                        if (AttrArr[attr].vertType == 1301)
                                            vert.pos = new Vector3 { X = f.readHalfFloat(), Y = f.readHalfFloat(), Z = f.readHalfFloat() };
                                        if (AttrArr[attr].vertType == 1304)
                                            vert.pos = new Vector3 { X = f.readFloat(), Y = f.readFloat(), Z = f.readFloat() };
                                        break;
                                    case "_c0":
                                        if (AttrArr[attr].vertType == 1301)
                                            vert.col = new Vector4(f.readHalfFloat(), f.readHalfFloat(), f.readHalfFloat(), f.readHalfFloat());
                                        if (AttrArr[attr].vertType == 2067)
                                            vert.col = new Vector4 { X = f.readFloat(), Y = f.readFloat(), Z = f.readFloat(), W = f.readFloat() };
                                        if (AttrArr[attr].vertType == 267)
                                            vert.col = new Vector4(f.readByte() / 255f, f.readByte() / 255f, f.readByte() / 255f, f.readByte() / 255f);
                                        break;
                                    case "_n0":
                                        if (AttrArr[attr].vertType == 526)
                                        {
                                            int normVal = (int)f.readInt();
                                            //Thanks RayKoopa!
                                            vert.nrm = new Vector3 { X = sign10Bit((normVal) & 0x3FF) / (float)511, Y = sign10Bit((normVal >> 10) & 0x3FF) / (float)511, Z = sign10Bit((normVal >> 20) & 0x3FF) / (float)511 };
                                        }
                                        break;
                                    case "_u0":
                                    case "color":
                                    case "_t0":
                                    case "_b0":
                                    case "_u1":
                                    case "_u2":
                                    case "_u3":
                                        if (AttrArr[attr].vertType == 265 || AttrArr[attr].vertType == 521)
                                            vert.tx.Add(new Vector2 { X = ((float)f.readByte()) / 255, Y = ((float)f.readByte()) / 255 });
                                        if (AttrArr[attr].vertType == 274)
                                            vert.tx.Add(new Vector2 { X = ((float)f.readShort()) / 65535, Y = ((float)f.readShort()) / 65535 });
                                        if (AttrArr[attr].vertType == 530)
                                            vert.tx.Add(new Vector2 { X = ((float)f.readShort()) / 32767, Y = ((float)f.readShort()) / 32767 });
                                        if (AttrArr[attr].vertType == 1298)
                                            vert.tx.Add(new Vector2 { X = f.readHalfFloat(), Y = f.readHalfFloat() });
                                        if (AttrArr[attr].vertType == 1303)
                                            vert.tx.Add(new Vector2 { X = f.readFloat(), Y = f.readFloat() });
                                        break;
                                    case "_i0":
                                        if (AttrArr[attr].vertType == 770)
                                        {
                                            vert.node.Add(f.readByte());
                                            vert.weight.Add((float)1.0);
                                        }
                                        if (AttrArr[attr].vertType == 777)
                                        {
                                            vert.node.Add(f.readByte());
                                            vert.node.Add(f.readByte());
                                        }
                                        if (AttrArr[attr].vertType == 779)
                                        {
                                            vert.node.Add(f.readByte());
                                            vert.node.Add(f.readByte());
                                            vert.node.Add(f.readByte());
                                            vert.node.Add(f.readByte());
                                        }
                                        if (AttrArr[attr].vertType == 523)
                                        {
                                            vert.node.Add(f.readByte());
                                            vert.node.Add(f.readByte());
                                            vert.node.Add(f.readByte());
                                            vert.node.Add(f.readByte());
                                        }
                                        break;
                                    case "_w0":
                                        if (AttrArr[attr].vertType == 258)
                                        {
                                            vert.weight.Add((f.readByte()) / (float)255);
                                        }
                                        if (AttrArr[attr].vertType == 265)
                                        {
                                            vert.weight.Add((f.readByte()) / (float)255);
                                            vert.weight.Add((f.readByte()) / (float)255);
                                        }
                                        if (AttrArr[attr].vertType == 267)
                                        {
                                            vert.weight.Add((f.readByte()) / (float)255);
                                            vert.weight.Add((f.readByte()) / (float)255);
                                            vert.weight.Add((f.readByte()) / (float)255);
                                            vert.weight.Add((f.readByte()) / (float)255);
                                        }
                                        if (AttrArr[attr].vertType == 274)
                                        {
                                            vert.weight.Add((f.readShort()) / (float)255);
                                            vert.weight.Add((f.readShort()) / (float)255);
                                        }
                                        break;
                                    default:
                                        //     //Console.WriteLine(AttrArr[attr].attName + " Unknown type " + AttrArr[attr].vertType.ToString("x") + " 0x");
                                        break;
                                }
                            }
                            poly.vertices.Add(vert);
                        }
                        int LoadLOD = FSHPArr[m].lodMdlCount - 1;

                        f.seek((int)FSHPArr[m].lodMdlOff);
                        for (int lod = 0; lod < FSHPArr[m].lodMdlCount; lod++)
                        {
                            long SubMeshOff = f.readInt64();
                            long unk1 = f.readInt64();
                            long unk2 = f.readInt64();
                            long indxBuffOff = f.readInt64();
                            int FaceBuffer = f.readInt();
                            int PrimativefaceType = f.readInt();
                            int faceType = f.readInt();
                            int FaceCount = f.readInt();
                            int elmSkip = f.readInt();
                            int subMeshCount = f.readInt();

                            int temp = f.pos();



                            f.seek(FaceBuffer + DataStart);
                            if (faceType == 1)
                                FaceCount = FaceCount / 3;
                            if (faceType == 2)
                                FaceCount = FaceCount / 6;


                            if (lod == LoadLOD)
                            {
                                for (int face = 0; face < FaceCount; face++)
                                {
                                    if (faceType == 1)
                                        poly.faces.Add(new List<int> { elmSkip + f.readShort(), elmSkip + f.readShort(), elmSkip + f.readShort() });
                                    else if (faceType == 2)
                                        poly.faces.Add(new List<int> { elmSkip + f.readInt(), elmSkip + f.readInt(), elmSkip + f.readInt() });
                                    else
                                        Console.Write("UnkFaceFormat");
                                }
                            }

                            f.seek(temp);
                        }



                        f.seek((int)FMATheaders[FSHPArr[m].fmatIndx].texSelOff);
                        List<string> MatTexList = new List<string>();
                        for (int tex = 0; FMATheaders[FSHPArr[m].fmatIndx].texAttSelCount > tex; tex++)
                        {
                            string TextureName = f.readString((int)f.readInt64() + 2, -1).ToLower();
                            MatTexList.Add(TextureName);
                        }

                        if (MatTexList.Count > 0)
                            poly.texNames.Add(MatTexList[0]);

                        //Console.WriteLine(String.Join(",",MatTexList));

                        model.poly.Add(poly);
                    }
                    models.Add(model);
                    f.seek(NextFMDL);


                }
            }
        }        

        public class FMDLheader
        {
            public string magic;
            public int headerLength1;
            public long headerLength2;
            public string name;
            public long eofString;
            public long fsklOff;
            public long fvtxArrOff;
            public long fvtxOff;
            public long matrOff;
            public long fshpOffset;
            public long fshpIndx;
            public long fmatOffset;
            public long fmatIndx;
            public long UserDataOffset;
            public int fvtxCount;
            public int fshpCount;
            public int fmatCount;
            public int paramCount;
            public int VertCount;
            public int un;
            public int unk2;
            public int padding;
            public long padding1;
            public long padding2;
            public int padding3;
        }
        public class FVTXH
        {
            public int magic = 0x46565458;//FVTX
            public int attCount;
            public int buffCount;
            public int sectIndx;
            public int vertCount;
            public int u1;
            public long attArrOff;
            public long attIndxOff;
            public long buffArrOff;
            public long buffSizeOff;
            public long buffStrideSizeOff;
            public int buffOff;
            public long unk1;
            public long unk2;
            public long unk3;
            public int SkinWeightInfluence;
        }
        public class FMATH
        {
            public string name;
            public long renderInfoOff;
            public long renderInfoIndx;
            public long shaderAssignOff;
            public long u1;
            public long texSelOff;
            public long u2;
            public long texAttSelOff;
            public long texAttIndxOff;
            public long matParamArrOff;
            public long matParamIndxOff;
            public long matParamOff;
            public long userDataOff;
            public long userDataIndxOff;
            public int padding;
            public long volatileFlagOffset;
            public long u3;
            public long samplerSlotOff;
            public long textureSlotOff;
            public int flags;
            public int sectIndx;
            public int rendParamCount;
            public int texSelCount;
            public int texAttSelCount;
            public int matParamCount;
            public int matParamSize;
            public int rawParamDataSize;
            public int userDataCount;
            public int u4;
        }
        public class FSKLH
        {
            public string magic;
            public int HeaderLength1;
            public long HeaderLenght2;
            public long boneIndxOff;
            public long boneArrOff;
            public long invMatrArrOff;
            public long invIndxArrOff;
            public int fsklType; //Flags
            public int boneArrCount;
            public int invIndxArrCount;
            public int exIndxCount;
            public long padding1;
            public long padding2;
            public long padding3;
            public long padding4;
            public long padding5;
            public int u1;
        }
        public class FSHPH
        {
            public string Magic;
            public int polyNameOff;
            public long fvtxOff;
            public long lodMdlOff;
            public long fsklIndxArrOff; //Skin bone index list
            public long boundingBoxOff;
            public long radiusOff;
            public int flags;
            public int sectIndx;
            public int matrFlag;
            public int fmatIndx;
            public int fsklIndx;
            public int fvtxIndx;
            public int u1;
            public int fsklIndxArrCount;
            public int lodMdlCount;
            public int visGrpCount;
            public long u3;
            public long u4;
            public int visGrpNodeOff;
            public int visGrpRangeOff;
            public int visGrpIndxOff;
            public long padding;
            public int VertexSkinCount;
            public int padding2;
            public int[] Node_Array;
        }
        public class attdata
        {
            public string attName;
            public int buffIndx;
            public int buffOff;
            public int vertType;
        }
        public class buffData
        {
            public int buffSize;
            public int strideSize;
            public int DataOffset;
        }
        public class lodmdl
        {
            public int u1;
            public int faceType;
            public int dCount;
            public int visGrpCount;
            public int u3;
            public int visGrpOff;
            public int indxBuffOff;
            public int elmSkip;
        }
        public class Vertex
        {
            public Vector3 pos = new Vector3(0, 0, 0), nrm = new Vector3(0, 0, 0);
            public Vector4 col = new Vector4(2, 2, 2, 2);
            public List<Vector2> tx = new List<Vector2>();
            public List<int> node = new List<int>();
            public List<float> weight = new List<float>();
        }
        public class Mesh
        {
            public List<List<int>> faces = new List<List<int>>();
            public List<Vertex> vertices = new List<Vertex>();
            public List<string> texNames = new List<string>();
            public string name;
        }

        public class FMDL_Model
        {
            public List<Mesh> poly = new List<Mesh>();
            public bool isVisible = true;
            public int[] Node_Array;
        }

    }
}
