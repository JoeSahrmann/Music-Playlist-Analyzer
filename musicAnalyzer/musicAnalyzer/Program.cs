using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace musicAnalyzer
{
   
    class Program
    {

        public static List<MusicAnalyzerdata> MusicAnalyzersList = new List<MusicAnalyzerdata>();

        static void Main(string[] args)
        {
            string musicData = string.Empty;
            string output = string.Empty;
            string curent = Directory.GetCurrentDirectory();
            if (args.Length != 2)
            {
                Console.WriteLine("Invalid call\n Valid example given: <music_file_path> <report_file_path>");
                Console.ReadLine();

                return;
            }

            else
            {
                //assign playlist file
                musicData = args[0];
                //assign txt file
                output = args[1];

                if (!musicData.Contains("\\"))
                {
                    musicData = Path.Combine(curent, musicData);
                    
                }
                
                if (!output.Contains("\\"))
                {
                    output = Path.Combine(curent, output);
                }
            }
            //if the file is found
            if (File.Exists(musicData))
            {
                if (ReadMusicPlaylist(musicData))
                {
                    try

                    {
                        var file = File.Create(output);
                        file.Close();
                    }
                    catch (Exception uh)
                    {
                        Console.WriteLine("Unable to create report file at : {0}, do to {1}",output,uh);
                    }
                    finally
                    {
                        WriteMusicPlaylist(output);
                    }
                }
                else
                {
                    Console.Write("Crime data file does not exist at path: {0}", musicData);
                }
                Console.ReadLine();
            }
        }

        private static bool ReadMusicPlaylist(string music_data_file)
        {
            Console.WriteLine($"~~~~~~~ \n Reading data from file : \n {music_data_file}\n ~~~~~~~");
        
            try
            {
                int git = 0;
                string[] musixlinez = File.ReadAllLines(music_data_file);

                for (int index = 0; index < musixlinez.Length; index++)
                {
                    string musixline = musixlinez[index];
                    string[] tab = musixline.Split('\t');//this splits each variable by the tab into their own variables instead of being a giant string 
                    
                  
                    if (index == 0)
                    {
                        git = tab.Length;
                   

                    }
                    else
                    {
                        if (git != tab.Length)
                        {
                            Console.WriteLine("Row {0} contains {1} values. It should contain {2}.",index,tab.Length,git);

                            return false;
                        }
                        else
                        {
                            try
                            {
                               

                                MusicAnalyzerdata musix = new MusicAnalyzerdata();
                               
                                musix.Name = tab[0];//1
                                musix.Artist = tab[1];//2
                                musix.Album = tab[2];//3
                                musix.Genre = tab[3];//4
                                musix.Size = Convert.ToInt32(tab[4]);//5
                                musix.Time = Convert.ToInt32(tab[5]);//6
                                musix.Year = Convert.ToInt32(tab[6]);//7
                                musix.Plays = Convert.ToInt32(tab[7]);//8
                                
                                //add these to the list we created
                                MusicAnalyzersList.Add(musix); 
                            }
                
                            catch (InvalidCastException uho)
                            {
                                Console.WriteLine($"Row {index} contains invalid value{uho}.");

                                return false;
                            }

                        }

                    }

                }

                Console.WriteLine($"Data read completed successfully.");

                return true;
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error in reading data from Music data file.");
               

                throw ex; 
            }
           

        }
        private static void WriteMusicPlaylist(string output_data_file)
        {

            try
            {

                if (MusicAnalyzersList != null && MusicAnalyzersList.Any())
                {
                    Console.WriteLine($"Calculating the desired data and writing it to report file : "+ output_data_file);
                    StringBuilder outputstring = new StringBuilder();
                    outputstring.Append("~~~ Music Analyzer Report ~~~ \n");
                    outputstring.Append(Environment.NewLine);

                    //1. how many songs received 200 or more plays?
                    //create a variable for each tab delimited value
                    var Name200 = MusicAnalyzersList.Where(MusicAnalyzersList => MusicAnalyzersList.Plays >= 200).Select(MusicAnalyzersList => MusicAnalyzersList.Name).ToArray();
                    var Artist200 = MusicAnalyzersList.Where(MusicAnalyzersList => MusicAnalyzersList.Plays >= 200).Select(MusicAnalyzersList => MusicAnalyzersList.Artist).ToArray();
                    var Album200 = MusicAnalyzersList.Where(MusicAnalyzersList => MusicAnalyzersList.Plays >= 200).Select(MusicAnalyzersList => MusicAnalyzersList.Album).ToArray();
                    var Genre200 = MusicAnalyzersList.Where(MusicAnalyzersList => MusicAnalyzersList.Plays >= 200).Select(MusicAnalyzersList => MusicAnalyzersList.Genre).ToArray();
                    var Size200 = MusicAnalyzersList.Where(MusicAnalyzersList => MusicAnalyzersList.Plays >= 200).Select(MusicAnalyzersList => MusicAnalyzersList.Size).ToArray();
                    var Time200 = MusicAnalyzersList.Where(MusicAnalyzersList => MusicAnalyzersList.Plays >= 200).Select(MusicAnalyzersList => MusicAnalyzersList.Time).ToArray();
                    var Year200 = MusicAnalyzersList.Where(MusicAnalyzersList => MusicAnalyzersList.Plays >= 200).Select(MusicAnalyzersList => MusicAnalyzersList.Year).ToArray();
                    var Plays200 = MusicAnalyzersList.Where(MusicAnalyzersList => MusicAnalyzersList.Plays >= 200).Select(MusicAnalyzersList => MusicAnalyzersList.Plays).ToArray();

                    outputstring.Append("1. Songs played over 200 times: " ); outputstring.Append(Environment.NewLine);

                    //goes through and sorts the arrays
                    for (int i = 0; i < Name200.Count(); i++)
                    {
                       //adds them to the output string
                        outputstring.Append("Name: " + Name200.ElementAt(i)+ ", ");
                        outputstring.Append("Artist: " + Artist200.ElementAt(i) + ", ");
                        outputstring.Append("Album: " + Album200.ElementAt(i) + ", ");
                        outputstring.Append("Genre: " + Genre200.ElementAt(i) + ", ");
                        outputstring.Append("Size: " + Size200.ElementAt(i) + ", ");
                        outputstring.Append("Time: " + Time200.ElementAt(i) + ", ");
                        outputstring.Append("Year: " + Year200.ElementAt(i) + ", ");
                        outputstring.Append("Plays: " + Plays200.ElementAt(i));
                        outputstring.Append(Environment.NewLine);
                         

                    }
                   
                    outputstring.Append(Environment.NewLine);

                    //2. How many songs are in the playlist with the Genre of "Alternative"
                    
                    var NumofAlt = MusicAnalyzersList.Where(MusicAnalyzersList => MusicAnalyzersList.Genre =="Alternative" ).ToArray();
                    outputstring.Append("2.Number of alternative songs: "+ NumofAlt.Count()+ "\n");

                    outputstring.Append(Environment.NewLine);

                    //3. How many songs are in the playlist with the Genre of "Hip-Hop/Rap"
                    var NumofRap = MusicAnalyzersList.Where(MusicAnalyzersList => MusicAnalyzersList.Genre == "Hip-Hop/Rap").ToArray();
                    outputstring.Append("3.Number of Hip-Hop/Rap songs: " + NumofRap.Count() + "\n");

                    outputstring.Append(Environment.NewLine);

                    //4. What songs are from the album "welcome to the fishbowl
                    outputstring.Append("4.What songs are from the album welcome to the fishbowl:  "); outputstring.Append(Environment.NewLine);

                    var fishbowl = MusicAnalyzersList.Where(MusicAnalyzersList => MusicAnalyzersList.Album == "Welcome to the Fishbowl").Select(MusicAnalyzersList => MusicAnalyzersList.Name).ToArray();
                    var fbArtist = MusicAnalyzersList.Where(MusicAnalyzersList => MusicAnalyzersList.Album == "Welcome to the Fishbowl").Select(MusicAnalyzersList => MusicAnalyzersList.Artist).ToArray();
                    var fbAlbum = MusicAnalyzersList.Where(MusicAnalyzersList => MusicAnalyzersList.Album == "Welcome to the Fishbowl").Select(MusicAnalyzersList => MusicAnalyzersList.Album).ToArray();
                    var fbGenre = MusicAnalyzersList.Where(MusicAnalyzersList => MusicAnalyzersList.Album == "Welcome to the Fishbowl").Select(MusicAnalyzersList => MusicAnalyzersList.Genre).ToArray();
                    var fbSize = MusicAnalyzersList.Where(MusicAnalyzersList => MusicAnalyzersList.Album == "Welcome to the Fishbowl").Select(MusicAnalyzersList => MusicAnalyzersList.Size).ToArray();
                    var fbTime = MusicAnalyzersList.Where(MusicAnalyzersList => MusicAnalyzersList.Album == "Welcome to the Fishbowl").Select(MusicAnalyzersList => MusicAnalyzersList.Time).ToArray();
                    var fbYear = MusicAnalyzersList.Where(MusicAnalyzersList => MusicAnalyzersList.Album == "Welcome to the Fishbowl").Select(MusicAnalyzersList => MusicAnalyzersList.Year).ToArray();
                    var fbPlays = MusicAnalyzersList.Where(MusicAnalyzersList => MusicAnalyzersList.Album == "Welcome to the Fishbowl").Select(MusicAnalyzersList => MusicAnalyzersList.Plays).ToArray();
                    for (int x = 0; x < fishbowl.Count(); x++)
                    {
                        //adds them to the output string
                        outputstring.Append("Name: " + fishbowl.ElementAt(x) + ", ");
                        outputstring.Append("Artist: " + fbArtist.ElementAt(x) + ", ");
                        outputstring.Append("Album: " + fbAlbum.ElementAt(x) + ", ");
                        outputstring.Append("Genre: " + fbGenre.ElementAt(x) + ", ");
                        outputstring.Append("Size: " + fbSize.ElementAt(x) + ", ");
                        outputstring.Append("Time: " + fbTime.ElementAt(x) + ", ");
                        outputstring.Append("Year: " + fbYear.ElementAt(x) + ", ");
                        outputstring.Append("Plays: " + fbPlays.ElementAt(x));
                        outputstring.Append(Environment.NewLine);


                    }
                    outputstring.Append(Environment.NewLine);

                    //5.What are the songs in the playlist from before 1970?
                    outputstring.Append("5.What songs in the playlist from before 1970:  "); outputstring.Append(Environment.NewLine);
                    var Name70 = MusicAnalyzersList.Where(MusicAnalyzersList => MusicAnalyzersList.Year < 1970).Select(MusicAnalyzersList => MusicAnalyzersList.Name).ToArray();
                    var Artist70 = MusicAnalyzersList.Where(MusicAnalyzersList => MusicAnalyzersList.Year < 1970).Select(MusicAnalyzersList => MusicAnalyzersList.Artist).ToArray();
                    var Album70 = MusicAnalyzersList.Where(MusicAnalyzersList => MusicAnalyzersList.Year < 1970).Select(MusicAnalyzersList => MusicAnalyzersList.Album).ToArray();
                    var Genre70 = MusicAnalyzersList.Where(MusicAnalyzersList => MusicAnalyzersList.Year < 1970).Select(MusicAnalyzersList => MusicAnalyzersList.Genre).ToArray();
                    var Size70 = MusicAnalyzersList.Where(MusicAnalyzersList => MusicAnalyzersList.Year < 1970).Select(MusicAnalyzersList => MusicAnalyzersList.Size).ToArray();
                    var Time70 = MusicAnalyzersList.Where(MusicAnalyzersList => MusicAnalyzersList.Year < 1970).Select(MusicAnalyzersList => MusicAnalyzersList.Time).ToArray();
                    var Year70 = MusicAnalyzersList.Where(MusicAnalyzersList => MusicAnalyzersList.Year < 1970).Select(MusicAnalyzersList => MusicAnalyzersList.Year).ToArray();
                    var Plays70 = MusicAnalyzersList.Where(MusicAnalyzersList => MusicAnalyzersList.Year < 1970).Select(MusicAnalyzersList => MusicAnalyzersList.Plays).ToArray();
                    for (int x = 0; x < Name70.Count(); x++)
                    {
                        //adds them to the output string
                        outputstring.Append("Name: " + Name70.ElementAt(x) + ", ");
                        outputstring.Append("Artist: " + Artist70.ElementAt(x) + ", ");
                        outputstring.Append("Album: " + Album70.ElementAt(x) + ", ");
                        outputstring.Append("Genre: " + Genre70.ElementAt(x) + ", ");
                        outputstring.Append("Size: " + Size70.ElementAt(x) + ", ");
                        outputstring.Append("Time: " + Time70.ElementAt(x) + ", ");
                        outputstring.Append("Year: " + Year70.ElementAt(x) + ", ");
                        outputstring.Append("Plays: " + Plays70.ElementAt(x));
                        outputstring.Append(Environment.NewLine);


                    }
                    outputstring.Append(Environment.NewLine);

                    //6. Song names longer than 85 characters
                    outputstring.Append("6.Song names longer than 85 characters:  "); outputstring.Append(Environment.NewLine);
                    var Longsong = MusicAnalyzersList.Where(MusicAnalyzersList => MusicAnalyzersList.Name.Length > 85).Select(MusicAnalyzersList => MusicAnalyzersList.Name).ToArray();
                    for (int x = 0; x < Longsong.Count(); x++)
                    {
                        outputstring.Append(Longsong.ElementAt(x) + "\n");
                    }
                    outputstring.Append(Environment.NewLine);

                    //7.Longest song in time
                    outputstring.Append("7.Longest song(in time):  "); outputstring.Append(Environment.NewLine);
                    var LongTimesong = MusicAnalyzersList.OrderByDescending(z =>z.Time).Select(MusicAnalyzersList => MusicAnalyzersList.Name).ToArray();
                    var LongTimesongArtist = MusicAnalyzersList.OrderByDescending(z => z.Time).Select(MusicAnalyzersList => MusicAnalyzersList.Artist).ToArray();
                    var LongTimesongAlbum = MusicAnalyzersList.OrderByDescending(z => z.Time).Select(MusicAnalyzersList => MusicAnalyzersList.Album).ToArray();
                    var LongTimesongGenre = MusicAnalyzersList.OrderByDescending(z => z.Time).Select(MusicAnalyzersList => MusicAnalyzersList.Genre).ToArray();
                    var LongTimesongSize = MusicAnalyzersList.OrderByDescending(z => z.Time).Select(MusicAnalyzersList => MusicAnalyzersList.Size).ToArray();
                    var LongTimesongTime = MusicAnalyzersList.OrderByDescending(z => z.Time).Select(MusicAnalyzersList => MusicAnalyzersList.Time).ToArray();
                    var LongTimesongYear = MusicAnalyzersList.OrderByDescending(z => z.Time).Select(MusicAnalyzersList => MusicAnalyzersList.Year).ToArray();
                    var LongTimesongPlays = MusicAnalyzersList.OrderByDescending(z => z.Time).Select(MusicAnalyzersList => MusicAnalyzersList.Plays).ToArray();

                    outputstring.Append("Name: " + LongTimesong.ElementAt(0) + ", ");
                    outputstring.Append("Artist: " + LongTimesongArtist.ElementAt(0) + ", ");
                    outputstring.Append("Album: " + LongTimesongAlbum.ElementAt(0) + ", ");
                    outputstring.Append("Genre: " + LongTimesongGenre.ElementAt(0) + ", ");
                    outputstring.Append("Size: " + LongTimesongSize.ElementAt(0) + ", ");
                    outputstring.Append("Time: " + LongTimesongTime.ElementAt(0) + ", ");
                    outputstring.Append("Year: " + LongTimesongYear.ElementAt(0) + ", ");
                    outputstring.Append("Plays: " + LongTimesongPlays.ElementAt(0));
                    outputstring.Append(Environment.NewLine);



                    using (var stream = new StreamWriter(output_data_file))
                    {
                        stream.Write(outputstring.ToString());
                    }
                    Console.WriteLine();
                    Console.WriteLine(outputstring.ToString());
                    Console.WriteLine();
                    Console.WriteLine($"Written report file successfully at : {output_data_file}");
                }

                else
                {
                    Console.WriteLine($"No data to write.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in writing report file.");

                throw ex;
            }
        }
    }
}
    public class MusicAnalyzerdata
    {
        public string Name { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Genre { get; set; }
        public int Size { get; set; }
        public int Time { get; set; }
        public int Year { get; set; }
        public int Plays { get; set; }
      
    }
    

