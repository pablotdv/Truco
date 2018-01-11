using System.Collections.Generic;
using Truco.Backend.Controllers;
using Xunit;
using System.Linq;

namespace Truco.Backend.Tests.Controllers
{
    public class ValuesControllerTest
    {        
        [Fact]
        public void Get_ShouldReturnListOfStrings()
        {
            // Arrange
            var controller = new ValuesController();

            // Act
            var result = controller.Get().ToList();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("value1", result[0]);
            Assert.Equal("value2", result[1]);
        }

        [Fact]
        public void Get_ShouldReturnString()
        {
            // Arrange
            var controller = new ValuesController();

            // Act
            var result = controller.Get(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("value1", result);
        }
        
    }
}
