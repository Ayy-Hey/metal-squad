#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class AppleTangle
    {
        private static byte[] data = System.Convert.FromBase64String("TgSDax4ilP87ib/EKFLOCYgPmzipV/X5jOewpuHuhCNHe9PLt1Mln8By9EvAcvNTUPPy8fLy8fLA/fb5isBy8YbA/vbzpe3/8fEP9PTz8vGJ0JGDg4WdlYPQkZOTlYCEkZ6TldQSGyFHgC//tRHXOgGdiB0XRefn+Nv28fX19/Lx5u6YhISAg8rf34eXf/hE0Ac7XNzQn4BGz/HAfEezPzCTw4cHyvfcphsq/9H+KkqD6b9FmZaZk5GEmZ+e0LGFhJifgpmEicH09uPypaPB48Dh9vOl9Prj+rGAgJKcldCDhJGelJGClNCElYKdg9CROemCBa3+JY+vawLV80qlf72t/QHN1pfQesOaB/1yPy4bU98Jo5qrlHvpeS4Ju5wF91vSwPIY6M4IoPkjgpGThJmTldCDhJGElZ2VnoSD3sDvYSvut6Ab9R2uiXTdG8ZSp7ylHPiuwHLx4fbzpe3Q9HLx+MBy8fTAf4NxkDbrq/nfYkIItLgAkMhu5QVbU4Fit6OlMV/fsUMICxOAPRZTvJTF0+W75antQ2QHBmxuP6BKMaig2na4dgf98fH19fDAksH7wPn286XWwNT286X0++PtsYCAnJXQs5WChMDh9vOl9Prj+rGAgJyV0Lmek97Bj7FYaAkhOpZs1JvhIFNLFOvaM+/esFYHt72P+K7A7/bzpe3T9OjA5vbA//bzpe3j8fEP9PXA8/HxD8DtgJyV0LOVgoSZlpmTkYSZn57QsYXFwsHEwMPGquf9w8XAwsDJwsHEwObA5PbzpfTz4/2xgICcldCin5+EWCyO0sU61SUp/yabJFLU0+EHUVyAnJXQop+fhNCzscDu5/3AxsDEwkfrTWOy1OLaN//tRr1srpM4u3Dn/fb52na4dgf98fH19fDzcvHx8KzDxqrAksH7wPn286X09uPypaPB4ynGjzF3pSlXaUnCsgsoJYFujlGicOTbIJm3ZIb5DgSbfd6wVge3vY+EmJ+CmYSJwebA5PbzpfTz4/2xgN/AcTP2+Nv28fX19/LywHFG6nFDopWcmZGek5XQn57QhJiZg9CTlYL286Xt/vTm9OTbIJm3ZIb5DgSbfYeH3pGAgJyV3pOfnd+RgICclZORZW6K/FS3e6sk5sfDOzT/vT7kmSHQs7HAcvHSwP32+dp2uHYH/fHx8UHAqByq9MJ8mEN/7S6Vgw+XrpVM73VzdetpzbfHAllrsH7cJEFg4ihFyl0E//7wYvtB0ebehCXM/SuS5vXw83Lx//DAcvH68nLx8fAUYVn5npTQk5+elJmEmZ+eg9CfltCFg5WcldC5npPewdbA1PbzpfT74+2xgISZlpmTkYSV0JKJ0JGeidCAkYKE/23NA9u52Oo4Dj5FSf4pruwmO83GabzdiEcdfGssA4drAoYih8C/MdzQk5WChJmWmZORhJXQgJ+cmZOJ0J+W0ISYldCEmJWe0JGAgJyZk5H3HI3Jc3uj0CPINEFPar/6mw/bDNCRnpTQk5WChJmWmZORhJmfntCAcvHw9vnadrh2B5OU9fHAcQLA2va1ju+8m6BmsXk0hJL74HOxd8N6cbkohm/D5JVRh2Q53fLz8fDxU3LxoFp6JSoUDCD598dAhYXR");
        private static int[] order = new int[] { 50,52,50,28,40,53,35,21,57,11,50,42,19,56,16,46,46,33,55,24,20,52,50,42,33,31,29,40,52,33,54,35,59,47,57,58,59,53,53,53,54,59,55,56,47,49,55,55,56,52,53,53,56,55,59,55,56,57,58,59,60 };
        private static int key = 240;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
