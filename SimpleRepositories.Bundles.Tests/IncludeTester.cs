namespace SimpleRepositories.Bundles.Tests;

[TestClass]
public class IncludeTester
{
    [TestClass]
    public class FetchById : Tester<IncludableGarbageRepository>
    {
        [TestMethod]
        public void WhenNoIncludeRequested_DoNotPopulateProjection()
        {
            //Act
            var result = Instance.FetchById(1);

            //Assert
            result.Name.Should().BeNull();
        }

        [TestMethod]
        public void WhenIncludeRequested_PopulateProjection()
        {
            //Act
            var result = Instance.FetchById(1, Include.Name);

            //Assert
            result.Name.Should().Be($"{IncludableGarbageRepository.NamePrefix}100");
        }

        [TestMethod]
        public void WhenUnrelatedIncludeRequested_DoNotPopulateName()
        {
            //Act
            var result = Instance.FetchById(1, Include.Description);

            //Assert
            result.Name.Should().BeNull();
        }
    }

    [TestClass]
    public class FetchAll : Tester<IncludableGarbageRepository>
    {
        [TestMethod]
        public void WhenNoIncludeRequested_LeaveAllProjectionsNull()
        {
            //Act
            var result = Instance.FetchAll();

            //Assert
            result.Should().OnlyContain(x => x.Name == null);
        }

        [TestMethod]
        public void WhenIncludeRequested_PopulateEveryProjection()
        {
            //Act
            var result = Instance.FetchAll(Include.Name);

            //Assert
            result.Should().OnlyContain(x => x.Name == $"{IncludableGarbageRepository.NamePrefix}{x.NameId}");
        }
    }

    [TestClass]
    public class FetchByPredicate : Tester<IncludableGarbageRepository>
    {
        [TestMethod]
        public void WhenIncludeRequested_PopulateProjection()
        {
            //Act
            var result = Instance.Fetch(x => x.Id == 2, Include.Name);

            //Assert
            result.Name.Should().Be($"{IncludableGarbageRepository.NamePrefix}200");
        }
    }

    [TestClass]
    public class FetchManyById : Tester<IncludableGarbageRepository>
    {
        [TestMethod]
        public void WhenIncludeRequested_PopulateEveryProjection()
        {
            //Act
            var result = Instance.FetchManyById(new[] { 1, 3 }, Include.Name);

            //Assert
            result.Should().OnlyContain(x => x.Name == $"{IncludableGarbageRepository.NamePrefix}{x.NameId}");
        }
    }

    [TestClass]
    public class TryFetchById : Tester<IncludableGarbageRepository>
    {
        [TestMethod]
        public void WhenIncludeRequestedAndFound_PopulateProjection()
        {
            //Act
            var result = Instance.TryFetchById(1, Include.Name);

            //Assert
            result.Value!.Name.Should().Be($"{IncludableGarbageRepository.NamePrefix}100");
        }
    }

    [TestClass]
    public class DefaultBehavior : Tester<PlainIncludableGarbageRepository>
    {
        [TestMethod]
        public void WhenRepositoryDoesNotOverrideApplyIncludes_IncludeIsNoOp()
        {
            //Act
            var result = Instance.FetchById(1, Include.Name);

            //Assert
            result.Name.Should().BeNull();
        }
    }

    [TestClass]
    public class IncludeValue
    {
        [TestMethod]
        public void WhenKeysAreEqual_IncludesAreEqual()
        {
            //Act
            var result = new Include("Name") == Include.Name;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenKeysDiffer_IncludesAreNotEqual()
        {
            //Act
            var result = Include.Name == Include.Description;

            //Assert
            result.Should().BeFalse();
        }
    }
}
