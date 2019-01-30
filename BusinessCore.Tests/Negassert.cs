using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessCore.Tests
{
    class Negassert
    {
        public static void mustFail(Action instructionBlock)
        {
            try
            {
                instructionBlock();
                Assert.Fail();
            }
            catch (UnitTestAssertException)
            {
                throw;
            }
            catch (NotImplementedException)
            {
                throw;
            }
            catch (Exception)
            {
                //good
            }
        }
    }
}