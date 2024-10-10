// License:
// 
// Syldeyn
// Import Process
// 
// 
// 
// 	(c) Jeroen P. Broks, 2024
// 
// 		This program is free software: you can redistribute it and/or modify
// 		it under the terms of the GNU General Public License as published by
// 		the Free Software Foundation, either version 3 of the License, or
// 		(at your option) any later version.
// 
// 		This program is distributed in the hope that it will be useful,
// 		but WITHOUT ANY WARRANTY; without even the implied warranty of
// 		MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// 		GNU General Public License for more details.
// 		You should have received a copy of the GNU General Public License
// 		along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 
// 	Please note that some references to data like pictures or audio, do not automatically
// 	fall under this licenses. Mostly this is noted in the respective files.
// 
// Version: 24.10.10 I 
// End License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TrickyUnits;

namespace Syldeyn {
	internal class Import_Process {

		public readonly string Dir;
		public string MKLFile => Dir + "/License.MKL.gini";
		public string SyldeynFile => Dir + "/MKL_Syldeyn.ini"; // The MKL_ prefix is because most of my older projects will then automatically ignore the file when updating my git repositories. Saves me a lot of trouble.

		TGINI MKLData;
		GINIE SyldeynData;

		public Import_Process(string _dir) {
			try {
				QCol.Doing("Checking", _dir); Dir = _dir.Replace("\\", "/");
				if (!File.Exists(MKLFile)) { QCol.QuickError($"File \"{MKLFile}\" does not exist."); return; }
				if (File.Exists(SyldeynFile)) {
					QCol.QuickError($"The file \"{SyldeynFile}\" exists!");
					QCol.Cyan("\x07Overwrite? ");
					var r = Console.ReadKey(true); if (r.Key != ConsoleKey.Y) { QCol.Red("No\n\n"); return; }
					QCol.Green("Yes\n\n");
				}
				MKLData = GINI.ReadFromFile(MKLFile);
				SyldeynData = GINIE.FromSource($"# Nothing to see here\n[::SYLDEYN::]\nImported=MKL\nCreated={DateTime.Now}\nProject={MKLData["Project"]}\n");
				foreach (var s in MKLData.List("SkipFile")) {
					QCol.Doing("= Skip", s);
					SyldeynData[s, "Allow"] = "False";
				}
				foreach(var sd in MKLData.List("SkipDir")) {
					QCol.Doing("= BanDir", sd);
					SyldeynData.ListAddNew(":DIRS:", "Ignore", sd);
				}
				foreach (var d in MKLData.List("Known")) {
					QCol.Doing("= Convert", d);
					SyldeynData[d, "Known"] = "True";
					SyldeynData[d, "Allow"] = "True";
					SyldeynData[d, "md5"] = MKLData[$"Hash {d}"];
					SyldeynData[d, "size"] = MKLData[$"Size {d}"];
					SyldeynData[d, "time.cs"] = MKLData[$"C#TM {d}"];
					var fct = File.GetCreationTimeUtc($"{Dir}/{d}");
					SyldeynData[d, "time.lng"] = fct.ToFileTime().ToString();
					SyldeynData[d, "time.ignore"] = "True";
					SyldeynData[d, "LicenseByName"] = MKLData[$"Lic {d}"];
					SyldeynData[d, "iYear"] = MKLData[$"IYEAR {d}"];
					SyldeynData[d, "CYear"] = MKLData[$"IYEAR {d}"];
					var PF = $"LICFIELD - {d.ToUpper()} - {MKLData[$"Lic {d}"].ToUpper()} - ";
					//QCol.Doing("Debug", PF);
					foreach( var v in MKLData.Vars()) {
						//QCol.Doing("->", $"PF={PF}; v={v}; Prefixed={v.StartsWith(PF)}");
						if (v.StartsWith(PF)) {
							var fld=v.Substring(PF.Length).Trim();
							SyldeynData[d, $"Fld.{fld}"] = MKLData[v];
						}
					}
				}
				QCol.Doing("Disposing", MKLFile);
				File.Move(MKLFile, Dir + "/MKL_Backup/" + qstr.StripDir(MKLFile));
			} catch(Exception e) {
				QCol.QuickError($".NET error: {e.Message}");
			} finally {
				QCol.Doing("Saving",SyldeynFile);
				if (SyldeynData!=null) SyldeynData.SaveSource(SyldeynFile);
			}
		}
	}
}
