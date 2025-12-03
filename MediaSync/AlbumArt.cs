using ATL;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MediaSync
{
    internal class AlbumArt
    {
        /// <summary>
        /// Loads album cover from MP3 file directly into a PictureBox
        /// </summary>
        /// <param name="pictureBox">The PictureBox control to display the image</param>
        /// <param name="mp3FilePath">Path to the MP3 file</param>
        /// <returns>True if successful, false otherwise</returns>
        public static bool LoadAlbumArtToPictureBox(PictureBox pictureBox, string mp3FilePath)
        {
            if (pictureBox == null)
            {
                throw new ArgumentNullException(nameof(pictureBox));
            }

            if (!File.Exists(mp3FilePath))
            {
                return false;
            }

            try
            {
                // Dispose of existing image to prevent memory leaks
                if (pictureBox.Image != null)
                {
                    var oldImage = pictureBox.Image;
                    pictureBox.Image = null;
                    oldImage.Dispose();
                }

                // Load the track using ATL
                Track track = new Track(mp3FilePath);

                // Get all embedded pictures
                var pictures = track.EmbeddedPictures;

                if (pictures != null && pictures.Count > 0)
                {
                    // Try to find the front cover first
                    var frontCover = pictures.FirstOrDefault(p =>
                        p.PicType == PictureInfo.PIC_TYPE.Front ||
                        p.PicType == PictureInfo.PIC_TYPE.Generic);

                    // If no front cover, get the first available picture
                    var picture = frontCover ?? pictures[0];

                    // Convert picture data to Image and assign to PictureBox
                    using (MemoryStream ms = new MemoryStream(picture.PictureData))
                    {
                        // Create a copy of the image so we can dispose the MemoryStream
                        pictureBox.Image = new Bitmap(Image.FromStream(ms));
                    }

                    // Optional: Set SizeMode for better display
                    pictureBox.SizeMode = PictureBoxSizeMode.Zoom;

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading album art: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Loads album art with a fallback default image
        /// </summary>
        /// <param name="pictureBox">The PictureBox control to display the image</param>
        /// <param name="mp3FilePath">Path to the MP3 file</param>
        /// <param name="defaultImage">Default image to show if no album art is found</param>
        public static void LoadAlbumArtOrDefault(PictureBox pictureBox, string mp3FilePath, Image defaultImage)
        {
            bool success = LoadAlbumArtToPictureBox(pictureBox, mp3FilePath);

            if (!success && defaultImage != null)
            {
                // Dispose of existing image
                if (pictureBox.Image != null)
                {
                    var oldImage = pictureBox.Image;
                    pictureBox.Image = null;
                    oldImage.Dispose();
                }

                pictureBox.Image = new Bitmap(defaultImage);
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }
    }
}
