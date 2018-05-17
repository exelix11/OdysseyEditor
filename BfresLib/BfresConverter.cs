#define FIRST_MIPMAP_ONLY //If removed fix mtl generation

using BnTxx;
using BnTxx.Formats;
using Smash_Forge;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BfresLib
{
    public class BfresConverter
    {

        public static bool Convert(byte[] bfres, string outPath)
        {
            if (bfres == null) return false;
            BFRES s = new BFRES();
            s.Read(bfres);
            Export(outPath, s);
            GC.Collect();
            if (s.models.Count == 0) return false;
            return true;
        }

        public static bool GetTextures(byte[] bfres, string Folder)
        {
            if (bfres == null) return false;
            BFRES s = new BFRES();
            s.Read(bfres);
            ExportTextures(s, Folder);
            GC.Collect();
            if (s.textures.Count == 0) return false;
            return true;
        }

        //OBJ
        const string texFmt = "bmp";
        const string textureFolder = "GameTextures";
        internal static void Export(string FileName, BFRES model)
        {
            bool UseEmptyMat = false;
            List<string> ExportMats = new List<string>();
            if (model.models.Count > 0)
            {
                using (System.IO.StreamWriter f = new System.IO.StreamWriter(FileName))
                {
                    f.WriteLine($"mtllib {Path.GetFileNameWithoutExtension(FileName)}.mtl");
                    int vertexOffest = 1;
                    foreach (var mesh in model.models[0].poly)
                    {
                        bool NoTexture = mesh.vertices[0].tx.Count == 0;
                        foreach (var v in mesh.vertices)
                        {
                            f.WriteLine($"v {v.pos.X} {v.pos.Y} {v.pos.Z}"); //{v.col.X} {v.col.Y} {v.col.Z} are vertex colors (unsupported for OBJ)
                            if (!NoTexture)
                                f.WriteLine($"vt {v.tx[0].X } {1 - v.tx[0].Y}");
                            else
                                f.WriteLine($"vt 0 0"); //Or else offsets won't match
                            f.WriteLine($"vn {v.nrm.X} {v.nrm.Y} {v.nrm.Z}");
                        }

                        if (mesh.texNames.Count == 0)
                        {
                            UseEmptyMat = true;
                            NoTexture = true;
                            f.WriteLine($"usemtl OdysseyEditor_EmptyMat");
                        }
                        else
                        {
                            foreach (string m in mesh.texNames)
                                if (!ExportMats.Contains(m)) ExportMats.Add(m);
                            f.WriteLine($"usemtl {mesh.texNames[0]}");
                        }

                        for (int i = 0; i < mesh.faces.Count; i++)
                        {
                            var verts = mesh.faces[i];
                            //Debug.Assert(verts[0] == verts[1] && verts[2] == verts[1]);
                            int val = verts[0] + vertexOffest;
                            int val1 = verts[1] + vertexOffest;
                            int val2 = verts[2] + vertexOffest;
                            if (!NoTexture)
                                f.WriteLine($"f {val}/{val}/{val} {val1}/{val1}/{val1} {val2}/{val2}/{val2}");
                            else
                                f.WriteLine($"f {val}//{val} {val1}//{val1} {val2}//{val2}");
                        }
                        vertexOffest += mesh.vertices.Count;
                    }
                }

                using (System.IO.StreamWriter f = new System.IO.StreamWriter(FileName.Substring(0, FileName.Length - 3) + "mtl"))
                {
                    if (UseEmptyMat)
                    {
                        f.WriteLine($"newmtl OdysseyEditor_EmptyMat");
                        f.WriteLine($"Ka 0.000000 0.000000 0.000000");
                        f.WriteLine($"Kd 0.800000 0.800000 0.800000");
                        f.WriteLine($"Ks 0.0 0.0 0.0 \n");
                    }

                    foreach (string MatName in ExportMats)
                    {
                        if (!IsMaterialNameValid(MatName)) continue; //If a material texture is missing the mesh will not show, skip non "alb" materials
                        f.WriteLine($"newmtl {MatName}");
                        f.WriteLine($"Ka 0.000000 0.000000 0.000000");
                        f.WriteLine($"Kd 1.000000 1.000000 1.000000");
                        f.WriteLine($"Ks 0.0 0.0 0.0 ");
                        f.WriteLine($"map_Kd {textureFolder}/{MatName}.{texFmt}\n");
                    }
                }
            }

            ExportTextures(model, Path.GetDirectoryName(FileName));
        }

        static void ExportTextures(BFRES model,string ModelsFolder)
        {
            if (model.textures.Keys.Count > 0)
            {
                if (!Directory.Exists($"{ModelsFolder}/{textureFolder}"))
                    Directory.CreateDirectory($"{ModelsFolder}/{textureFolder}");

                foreach (string k in model.textures.Keys)
                {
                    foreach (var tex in model.textures[k].Textures.Where(
                        x => IsMaterialNameValid(x.Name)))
                    {
                        if (!File.Exists($"{ModelsFolder}/{textureFolder}/{tex.Name}.{texFmt}"))
                            ExportTexture(tex, $"{ModelsFolder}/{textureFolder}/{tex.Name}.{texFmt}");
                        else
                            Console.WriteLine("Skipped texture " + tex.Name);
                    }
                }
            }
        }

        static bool IsMaterialNameValid(string x)
        {
            return x.EndsWith("_alb", StringComparison.InvariantCultureIgnoreCase) ||
                   x.EndsWith("_alb.0", StringComparison.InvariantCultureIgnoreCase) ||
                   !x.Contains("_");
        }
        
        static void ExportTexture(Texture Tex, string FileName)
        {
#if !FIRST_MIPMAP_ONLY
            if (Tex.MipmapCount == 1)
            {
#endif
                if (PixelDecoder.TryDecode(Tex, out Bitmap Img))
                {
                    Img.Save(FileName);
                    Img.Dispose();
                }
                else
                {
                    Console.WriteLine("FAILED texture " + Tex.Name);
#if DEBUG
                    Debugger.Break();
#endif
                }
#if !FIRST_MIPMAP_ONLY
            }
            else
            {
                for (int Index = 0; Index < Tex.MipmapCount; Index++)
                {
                    if (!PixelDecoder.TryDecode(Tex, out Bitmap Img, (int)Tex.MipOffsets[Index]))
                    {
                        Debugger.Break();
                        break;
                    }

                    string Ext = Path.GetExtension(FileName);

                    Img.Save(FileName.Replace(Ext, "." + Index + "." + Ext));

                    Tex.Width = Math.Max(Tex.Width >> 1, 1);
                    Tex.Height = Math.Max(Tex.Height >> 1, 1);

                    while (Tex.GetBlockHeight() * 8 > Tex.GetPow2HeightInTexels() && Tex.BlockHeightLog2 > 0)
                    {
                        Tex.BlockHeightLog2--;
                    }
                }
            }
#endif
        }
    }
}