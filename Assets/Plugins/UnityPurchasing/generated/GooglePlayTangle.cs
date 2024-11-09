#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("RIaLdJgtjIjNe8pxOFnWc3WI3atSgr2cPl+CygW1kvckJuUcHck5vBIQUYxh4jscjYJCSipodVyyFD3dRMfJxvZEx8zERMfHxmwOEciqk7SIe2IUFUhAn0HYJRFqjyNeIDxKgdhU2t0DEb5HYddW0oDqhN9M3J2DBKYDLG+W737p/vs41gP0deEbvv1ak2PFInHUDe82f4yhhS9E8q+D9vZEx+T2y8DP7ECOQDHLx8fHw8bFJ+POjPKHCJVtnLbxHFPgNkuvaKIHzg4RQ+kASdKc/O/vaqq/9AldgcjO0n7dcZFT4++HoZXNJDENW2CloQP3E9nZ//R8zlVUWJ89QYl69lE18a4qorIxH+G6l4x7dxh0qOQkx8akmBjE7GeENcTFx8bH");
        private static int[] order = new int[] { 3,5,2,5,11,10,6,7,10,9,12,13,13,13,14 };
        private static int key = 198;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
