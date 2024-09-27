using SimpleRepositories.Bundles.Tests.GarbageTypes;

namespace SimpleRepositories.Bundles.Tests;

[TestClass]
public class ReadOnlyBundleRepositoryTester
{
    [TestClass]
    public class Indexer : Tester<GarbageReadOnlyBundleRepository>
    {
        [TestMethod]
        public void WhenNoEntityFound_Throw()
        {
            //Arrange
            var id = 0;

            //Act
            var action = () => Instance[id];

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.EntityWithIdNotFound, typeof(Garbage).GetHumanReadableName(), id));
        }

        [TestMethod]
        public void WhenEntityFound_Return()
        {
            //Arrange
            var id = 1;

            //Act
            var result = Instance[id];

            //Assert
            result.Should().Be(new Garbage
            {
                Id = 1,
                Name = "Jerry",
                Level = 20
            });
        }
    }

    [TestClass]
    public class Count_Parameterless : Tester<GarbageReadOnlyBundleRepository>
    {
        [TestMethod]
        public void Always_ReturnNumberOfItemsInTheEntireBundle()
        {
            //Arrange

            //Act
            var result = Instance.Count();

            //Assert
            result.Should().Be(4);
        }
    }

    [TestClass]
    public class Count_Predicate : Tester<GarbageReadOnlyBundleRepository>
    {
        [TestMethod]
        public void WhenPredicateIsEqualToSomething_ReturnCount()
        {
            //Arrange

            //Act
            var result = Instance.Count(x => x.Level <= 20);

            //Assert
            result.Should().Be(3);
        }

        [TestMethod]
        public void WhenPredicateDoesNotEqualAnything_ReturnZero()
        {
            //Arrange

            //Act
            var result = Instance.Count(x => x.Name == "Henry A. P.");

            //Assert
            result.Should().Be(0);
        }
    }

    [TestClass]
    public class Count_SubEntity : Tester<GarbageReadOnlyBundleRepository>
    {
        [TestMethod]
        public void Always_ReturnAmountOfType()
        {
            //Arrange

            //Act
            var result = Instance.Count<DerivedGarbage>();

            //Assert
            result.Should().Be(2);
        }
    }

    [TestClass]
    public class Count_Predicate_SubEntity : Tester<GarbageReadOnlyBundleRepository>
    {
        [TestMethod]
        public void Always_ReturnAmountOfTypeThatFitsPredicate()
        {
            //Arrange

            //Act
            var result = Instance.Count<DerivedGarbage>(x => x.Level > 10);

            //Assert
            result.Should().Be(1);
        }
    }

    [TestClass]
    public class FetchAll_Parameterless : Tester<GarbageReadOnlyBundleRepository>
    {
        [TestMethod]
        public void Always_ReturnEveryEntity()
        {
            //Arrange

            //Act
            var result = Instance.FetchAll();

            //Assert
            result.Should().BeEquivalentTo(GarbageReadOnlyBundleRepository.Items.Entities);
            result.Should().ContainInOrder(GarbageReadOnlyBundleRepository.Items.Entities.OrderBy(x => x.Id));
        }
    }

    [TestClass]
    public class FetchAll_SubEntity : Tester<GarbageReadOnlyBundleRepository>
    {
        [TestMethod]
        public void Always_ReturnEveryEntity()
        {
            //Arrange

            //Act
            var result = Instance.FetchAll<DerivedGarbage>();

            //Assert
            result.Should().BeEquivalentTo(new List<DerivedGarbage>
            {
                new()
                {
                    Id = 2,
                    Name = "Garry",
                    Level = 45,
                    Job = "Some guy"
                },
                new()
                {
                    Id = 4,
                    Name = "Harry",
                    Level = 7,
                    Job = "That guy over there"
                }
            });
        }
    }

    [TestClass]
    public class FetchAll_Predicate : Tester<GarbageReadOnlyBundleRepository>
    {
        [TestMethod]
        public void WhenNothingFitsPredicate_ReturnEmpty()
        {
            //Arrange

            //Act
            var result = Instance.FetchAll(x => x.Name == "Darryl");

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenOnlyOneItemFits_ReturnThatOneItem()
        {
            //Arrange

            //Act
            var result = Instance.FetchAll(x => x.Name == "Garry");

            //Assert
            result.Should().BeEquivalentTo(new List<Garbage>
            {
                new DerivedGarbage
                {
                    Id = 2,
                    Name = "Garry",
                    Level = 45,
                    Job = "Some guy"
                }
            });
        }

        [TestMethod]
        public void WhenMultipleItemsFit_ReturnAllOfThemOrderedById()
        {
            //Arrange

            //Act
            var result = Instance.FetchAll(x => x.Name.Contains("er"));

            //Assert
            result.Should().BeEquivalentTo(new List<Garbage>
            {
                new()
                {
                    Id = 1,
                    Name = "Jerry",
                    Level = 20
                },
                new()
                {
                    Id = 3,
                    Name = "Terry",
                    Level = 18
                },
            });
        }
    }

    [TestClass]
    public class FetchAll_Predicate_SubEntity : Tester<GarbageReadOnlyBundleRepository>
    {
        [TestMethod]
        public void Always_Return()
        {
            //Arrange

            //Act
            var result = Instance.FetchAll<DerivedGarbage>(x => x.Level > 10);

            //Assert
            result.Should().BeEquivalentTo(new List<DerivedGarbage>
            {
                new DerivedGarbage
                {
                    Id = 2,
                    Name = "Garry",
                    Level = 45,
                    Job = "Some guy"
                }
            });
        }
    }

    [TestClass]
    public class Fetch_Predicate : Tester<GarbageReadOnlyBundleRepository>
    {
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange
            Func<Garbage, bool> predicate = null!;

            //Act
            var action = () => Instance.Fetch(predicate);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("predicate");
        }

        [TestMethod]
        public void WhenThereIsExactlyOneItemFittingPredicate_ReturnThatItem()
        {
            //Arrange

            //Act
            var result = Instance.Fetch(x => x.Name == "Terry");

            //Assert
            result.Should().Be(new Garbage
            {
                Id = 3,
                Name = "Terry",
                Level = 18
            });
        }

        [TestMethod]
        public void WhenThereIsNoItemFittingPredicate_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.Fetch(x => x.Name == "Seb");

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.EntityWithPredicateNotFound, nameof(Garbage)));
        }

        [TestMethod]
        public void WhenThereIsMorethanOneItemFittingPredicate_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.Fetch(x => x.Level > 0);

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }
    }

    [TestClass]
    public class Fetch_Predicate_SubEntity : Tester<GarbageReadOnlyBundleRepository>
    {
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange
            Func<Garbage, bool> predicate = null!;

            //Act
            var action = () => Instance.Fetch<DerivedGarbage>(predicate);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("predicate");
        }

        [TestMethod]
        public void WhenTryingToFetchAnotherType_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.Fetch<DerivedGarbage>(x => x.Name == "Terry");

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.EntityWithPredicateNotFound, nameof(DerivedGarbage)));
        }

        [TestMethod]
        public void WhenThereIsMoreThanOneItemFittingPredicate_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.Fetch<DerivedGarbage>(x => x.Level > 0);

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void WhenThereIsOnlyOneItemFittingPredicate_ReturnThatItem()
        {
            //Arrange

            //Act
            var result = Instance.Fetch<DerivedGarbage>(x => x.Name == "Harry");

            //Assert
            result.Should().Be(new DerivedGarbage
            {
                Id = 4,
                Name = "Harry",
                Level = 7,
                Job = "That guy over there"
            });
        }
    }

    [TestClass]
    public class TryFetch_Predicate : Tester<GarbageReadOnlyBundleRepository>
    {
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange
            Func<Garbage, bool> predicate = null!;

            //Act
            var action = () => Instance.TryFetch(predicate);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("predicate");
        }

        [TestMethod]
        public void WhenThereIsExactlyOneItemFittingPredicate_ReturnThatItem()
        {
            //Arrange

            //Act
            var result = Instance.TryFetch(x => x.Name == "Terry");

            //Assert
            result.Should().Be(Result<Garbage>.Success(new Garbage
            {
                Id = 3,
                Name = "Terry",
                Level = 18
            }));
        }

        [TestMethod]
        public void WhenThereIsNoItemFittingPredicate_Throw()
        {
            //Arrange

            //Act
            var result = Instance.TryFetch(x => x.Name == "Seb");

            //Assert
            result.Should().Be(Result<Garbage>.Failure());
        }

        [TestMethod]
        public void WhenThereIsMorethanOneItemFittingPredicate_Throw()
        {
            //Arrange

            //Act
            var result = Instance.TryFetch(x => x.Level > 0);

            //Assert
            result.Should().Be(Result<Garbage>.Failure());
        }
    }

    [TestClass]
    public class TryFetch_Predicate_SubEntity : Tester<GarbageReadOnlyBundleRepository>
    {
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange
            Func<Garbage, bool> predicate = null!;

            //Act
            var action = () => Instance.TryFetch<DerivedGarbage>(predicate);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("predicate");
        }

        [TestMethod]
        public void WhenTryingToFetchAnotherType_Throw()
        {
            //Arrange

            //Act
            var result = Instance.TryFetch<DerivedGarbage>(x => x.Name == "Terry");

            //Assert
            result.Should().Be(Result<DerivedGarbage>.Failure());
        }

        [TestMethod]
        public void WhenThereIsMoreThanOneItemFittingPredicate_Throw()
        {
            //Arrange

            //Act
            var result = Instance.TryFetch<DerivedGarbage>(x => x.Level > 0);

            //Assert
            result.Should().Be(Result<DerivedGarbage>.Failure());
        }

        [TestMethod]
        public void WhenThereIsOnlyOneItemFittingPredicate_ReturnThatItem()
        {
            //Arrange

            //Act
            var result = Instance.TryFetch<DerivedGarbage>(x => x.Name == "Harry");

            //Assert
            result.Should().Be(Result<DerivedGarbage>.Success(new DerivedGarbage
            {
                Id = 4,
                Name = "Harry",
                Level = 7,
                Job = "That guy over there"
            }));
        }
    }

    [TestClass]
    public class Contains_Params : Tester<GarbageReadOnlyBundleRepository>
    {
        [TestMethod]
        public void WhenAllEntitiesInRepo_ReturnTrue()
        {
            //Arrange

            //Act
            var result = Instance.Contains(GarbageReadOnlyBundleRepository.Items.Entities.GetRandom()!, GarbageReadOnlyBundleRepository.Items.Entities.GetRandom()!, GarbageReadOnlyBundleRepository.Items.Entities.GetRandom()!);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenNotAllEntitiesInRepo_ReturnFalse()
        {
            //Arrange

            //Act
            var result = Instance.Contains(GarbageReadOnlyBundleRepository.Items.Entities.GetRandom()!, Dummy.Create<Garbage>(), GarbageReadOnlyBundleRepository.Items.Entities.GetRandom()!);

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenNoEntitiesInRepo_ReturnFalse()
        {
            //Arrange

            //Act
            var result = Instance.Contains(Dummy.Create<Garbage>(), Dummy.Create<Garbage>(), Dummy.Create<Garbage>());

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOneAndIsInRepo_ReturnTrue()
        {
            //Arrange

            //Act
            var result = Instance.Contains(GarbageReadOnlyBundleRepository.Items.Entities.GetRandom()!);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenOneAndIsNotInRepo_ReturnFalse()
        {
            //Arrange

            //Act
            var result = Instance.Contains(Dummy.Create<Garbage>());

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class Contains_Enumerable : Tester<GarbageReadOnlyBundleRepository>
    {
        [TestMethod]
        public void WhenEntitiesNull_Throw()
        {
            //Arrange
            IEnumerable<Garbage> entities = null!;

            //Act
            var action = () => Instance.Contains(entities);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(entities));
        }

        [TestMethod]
        public void WhenAllEntitiesInRepo_ReturnTrue()
        {
            //Arrange

            //Act
            var result = Instance.Contains(new List<Garbage>
            {
                GarbageReadOnlyBundleRepository.Items.Entities.GetRandom()!, GarbageReadOnlyBundleRepository.Items.Entities.GetRandom()!, GarbageReadOnlyBundleRepository.Items.Entities.GetRandom()!
            });

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenNotAllEntitiesInRepo_ReturnFalse()
        {
            //Arrange

            //Act
            var result = Instance.Contains(new List<Garbage>
            {
                GarbageReadOnlyBundleRepository.Items.Entities.GetRandom()!, Dummy.Create<Garbage>(), GarbageReadOnlyBundleRepository.Items.Entities.GetRandom()!
            });

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenNoEntitiesInRepo_ReturnFalse()
        {
            //Arrange

            //Act
            var result = Instance.Contains(new List<Garbage> { Dummy.Create<Garbage>(), Dummy.Create<Garbage>(), Dummy.Create<Garbage>() });

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenOneAndIsInRepo_ReturnTrue()
        {
            //Arrange

            //Act
            var result = Instance.Contains(new List<Garbage> { GarbageReadOnlyBundleRepository.Items.Entities.GetRandom()! });

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenOneAndIsNotInRepo_ReturnFalse()
        {
            //Arrange

            //Act
            var result = Instance.Contains(new List<Garbage> { Dummy.Create<Garbage>() });

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class Contains_Predicate : Tester<GarbageReadOnlyBundleRepository>
    {
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange
            Func<Garbage, bool> predicate = null!;

            //Act
            var action = () => Instance.Contains(predicate);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("predicate");
        }

        [TestMethod]
        public void WhenContainsExactlyOneItemThatFitsPredicate_ReturnTrue()
        {
            //Arrange

            //Act
            var result = Instance.Contains(x => x.Name == "Jerry");

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenContainsMultipleItemsThatFitPredicate_ReturnTrue()
        {
            //Arrange

            //Act
            var result = Instance.Contains(x => x.Name.Contains("ar"));

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenContainsNoItemThatFitsPredicate_ReturnFalse()
        {
            //Arrange

            //Act
            var result = Instance.Contains(x => x.Level < 0);

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class Contains_SubEntity : Tester<GarbageReadOnlyBundleRepository>
    {
        [TestMethod]
        public void WhenPredicateIsNull_Throw()
        {
            //Arrange
            Func<Garbage, bool> predicate = null!;

            //Act
            var action = () => Instance.Contains<DerivedGarbage>(predicate);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("predicate");
        }

        [TestMethod]
        public void WhenContainsExactlyOneItemThatFitsPredicate_ReturnTrue()
        {
            //Arrange

            //Act
            var result = Instance.Contains<DerivedGarbage>(x => x.Name == "Harry");

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenContainsMultipleItemsThatFitPredicate_ReturnTrue()
        {
            //Arrange

            //Act
            var result = Instance.Contains<DerivedGarbage>(x => x.Level > 5);

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenContainsNoItemThatFitsPredicate_ReturnFalse()
        {
            //Arrange

            //Act
            var result = Instance.Contains<DerivedGarbage>(x => x.Name == "Jerry");

            //Assert
            result.Should().BeFalse();
        }
    }

    [TestClass]
    public class FetchById : Tester<GarbageReadOnlyBundleRepository>
    {
        [TestMethod]
        public void WhenNoEntityFound_Throw()
        {
            //Arrange
            var id = 81;

            //Act
            var action = () => Instance.FetchById(id);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.EntityWithIdNotFound, typeof(Garbage).GetHumanReadableName(), id));
        }

        [TestMethod]
        public void WhenEntityFound_Return()
        {
            //Arrange
            var id = 2;

            //Act
            var result = Instance.FetchById(id);

            //Assert
            result.Should().Be(new DerivedGarbage
            {
                Id = 2,
                Name = "Garry",
                Level = 45,
                Job = "Some guy"
            });
        }
    }

    [TestClass]
    public class FetchById_SubEntity : Tester<GarbageReadOnlyBundleRepository>
    {
        [TestMethod]
        public void WhenEntityNotFound_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.FetchById<DerivedGarbage>(1);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.EntityWithIdNotFound, nameof(DerivedGarbage), 1));
        }

        [TestMethod]
        public void WhenEntityFound_Return()
        {
            //Arrange

            //Act
            var result = Instance.FetchById<DerivedGarbage>(2);

            //Assert
            result.Should().Be(new DerivedGarbage
            {
                Id = 2,
                Name = "Garry",
                Level = 45,
                Job = "Some guy"
            });
        }
    }

    [TestClass]
    public class TryFetchById : Tester<GarbageReadOnlyBundleRepository>
    {
        [TestMethod]
        public void WhenNoEntityFound_Throw()
        {
            //Arrange
            var id = 81;

            //Act
            var result = Instance.TryFetchById(id);

            //Assert
            result.Should().Be(Result<Garbage>.Failure());
        }

        [TestMethod]
        public void WhenEntityFound_Return()
        {
            //Arrange
            var id = 2;

            //Act
            var result = Instance.TryFetchById(id);

            //Assert
            result.Should().Be(Result<Garbage>.Success(new DerivedGarbage
            {
                Id = 2,
                Name = "Garry",
                Level = 45,
                Job = "Some guy"
            }));
        }
    }

    [TestClass]
    public class TryFetchById_SubEntity : Tester<GarbageReadOnlyBundleRepository>
    {
        [TestMethod]
        public void WhenEntityNotFound_Throw()
        {
            //Arrange

            //Act
            var result = Instance.TryFetchById<DerivedGarbage>(1);

            //Assert
            result.Should().Be(Result<DerivedGarbage>.Failure());
        }

        [TestMethod]
        public void WhenEntityFound_Return()
        {
            //Arrange

            //Act
            var result = Instance.TryFetchById<DerivedGarbage>(2);

            //Assert
            result.Should().Be(Result<DerivedGarbage>.Success(new DerivedGarbage
            {
                Id = 2,
                Name = "Garry",
                Level = 45,
                Job = "Some guy"
            }));
        }
    }

    [TestClass]
    public class FetchManyById_Params : Tester<GarbageReadOnlyBundleRepository>
    {
        [TestMethod]
        public void WhenNoIdProvided_ReturnEmpty()
        {
            //Arrange

            //Act
            var result = Instance.FetchManyById();

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenOnlyOneIdProvided_ReturnItemWithId()
        {
            //Arrange

            //Act
            var result = Instance.FetchManyById(3);

            //Assert
            result.Should().BeEquivalentTo(new List<Garbage>
            {
                new()
                {
                    Id = 3,
                    Name = "Terry",
                    Level = 18
                }
            });
        }

        [TestMethod]
        public void WhenMultipleIdsProvided_ReturnAllItemsWithIds()
        {
            //Arrange

            //Act
            var result = Instance.FetchManyById(1, 2, 4);

            //Assert
            result.Should().BeEquivalentTo(new List<Garbage>
            {
                new DerivedGarbage
                {
                    Id = 4,
                    Name = "Harry",
                    Level = 7,
                    Job = "That guy over there"
                },
                new()
                {
                    Id = 1,
                    Name = "Jerry",
                    Level = 20
                },
                new DerivedGarbage
                {
                    Id = 2,
                    Name = "Garry",
                    Level = 45,
                    Job = "Some guy"
                }
            });
        }

        [TestMethod]
        public void WhenIdsNotFound_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.FetchManyById(-1, 99, 56);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.EntityWithIdNotFound, nameof(Garbage), -1));
        }
    }

    [TestClass]
    public class FetchManyById_Enumerable : Tester<GarbageReadOnlyBundleRepository>
    {
        [TestMethod]
        public void WhenIdsNull_ReturnEmpty()
        {
            //Arrange
            IEnumerable<int> ids = null!;

            //Act
            var action = () => Instance.FetchManyById(ids);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("ids");
        }

        [TestMethod]
        public void WhenOnlyOneIdProvided_ReturnItemWithId()
        {
            //Arrange
            var ids = new List<int> { 3 };

            //Act
            var result = Instance.FetchManyById(ids);

            //Assert
            result.Should().BeEquivalentTo(new List<Garbage>
            {
                new()
                {
                    Id = 3,
                    Name = "Terry",
                    Level = 18
                }
            });
        }

        [TestMethod]
        public void WhenMultipleIdsProvided_ReturnAllItemsWithIds()
        {
            //Arrange
            var ids = new List<int> { 1, 2, 4 };

            //Act
            var result = Instance.FetchManyById(ids);

            //Assert
            result.Should().BeEquivalentTo(new List<Garbage>
            {
                new DerivedGarbage
                {
                    Id = 4,
                    Name = "Harry",
                    Level = 7,
                    Job = "That guy over there"
                },
                new()
                {
                    Id = 1,
                    Name = "Jerry",
                    Level = 20
                },
                new DerivedGarbage
                {
                    Id = 2,
                    Name = "Garry",
                    Level = 45,
                    Job = "Some guy"
                }
            });
        }

        [TestMethod]
        public void WhenIdsNotFound_Throw()
        {
            //Arrange
            var ids = new List<int> { -1, 99, 56 };

            //Act
            var action = () => Instance.FetchManyById(ids);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.EntityWithIdNotFound, nameof(Garbage), -1));
        }
    }

    [TestClass]
    public class FetchManyById_Params_SubEntity : Tester<GarbageReadOnlyBundleRepository>
    {
        [TestMethod]
        public void WhenNoIdProvided_ReturnEmpty()
        {
            //Arrange

            //Act
            var result = Instance.FetchManyById<DerivedGarbage>();

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenOnlyOneIdProvided_ReturnItemWithId()
        {
            //Arrange

            //Act
            var result = Instance.FetchManyById<DerivedGarbage>(2);

            //Assert
            result.Should().BeEquivalentTo(new List<Garbage>
            {
                new DerivedGarbage
                {
                    Id = 2,
                    Name = "Garry",
                    Level = 45,
                    Job = "Some guy"
                }
            });
        }

        [TestMethod]
        public void WhenMultipleIdsProvided_ReturnAllItemsWithIds()
        {
            //Arrange

            //Act
            var result = Instance.FetchManyById<DerivedGarbage>(2, 4);

            //Assert
            result.Should().BeEquivalentTo(new List<DerivedGarbage>
            {
                new DerivedGarbage
                {
                    Id = 4,
                    Name = "Harry",
                    Level = 7,
                    Job = "That guy over there"
                },
                new DerivedGarbage
                {
                    Id = 2,
                    Name = "Garry",
                    Level = 45,
                    Job = "Some guy"
                }
            });
        }

        [TestMethod]
        public void WhenIdsNotFound_Throw()
        {
            //Arrange

            //Act
            var action = () => Instance.FetchManyById<DerivedGarbage>(1, 3, 56);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.EntityWithIdNotFound, nameof(DerivedGarbage), 1));
        }
    }

    [TestClass]
    public class FetchManyById_Enumerable_SubEntity : Tester<GarbageReadOnlyBundleRepository>
    {
        [TestMethod]
        public void WhenNoIdProvided_Throw()
        {
            //Arrange
            IEnumerable<int> ids = null!;

            //Act
            var action = () => Instance.FetchManyById<DerivedGarbage>(ids);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("ids");
        }

        [TestMethod]
        public void WhenOnlyOneIdProvided_ReturnItemWithId()
        {
            //Arrange
            var ids = new List<int> { 2 };

            //Act
            var result = Instance.FetchManyById<DerivedGarbage>(ids);

            //Assert
            result.Should().BeEquivalentTo(new List<Garbage>
            {
                new DerivedGarbage
                {
                    Id = 2,
                    Name = "Garry",
                    Level = 45,
                    Job = "Some guy"
                }
            });
        }

        [TestMethod]
        public void WhenMultipleIdsProvided_ReturnAllItemsWithIds()
        {
            //Arrange
            var ids = new List<int> { 2, 4 };

            //Act
            var result = Instance.FetchManyById<DerivedGarbage>(ids);

            //Assert
            result.Should().BeEquivalentTo(new List<DerivedGarbage>
            {
                new DerivedGarbage
                {
                    Id = 4,
                    Name = "Harry",
                    Level = 7,
                    Job = "That guy over there"
                },
                new DerivedGarbage
                {
                    Id = 2,
                    Name = "Garry",
                    Level = 45,
                    Job = "Some guy"
                }
            });
        }

        [TestMethod]
        public void WhenIdsNotFound_Throw()
        {
            //Arrange
            var ids = new List<int> { 1, 3, 56 };

            //Act
            var action = () => Instance.FetchManyById<DerivedGarbage>(ids);

            //Assert
            action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.EntityWithIdNotFound, nameof(DerivedGarbage), 1));
        }
    }

    [TestClass]
    public class TryFetchManyById_Params : Tester<GarbageReadOnlyBundleRepository>
    {
        [TestMethod]
        public void WhenNoIdProvided_ReturnEmpty()
        {
            //Arrange

            //Act
            var result = Instance.TryFetchManyById();

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenOnlyOneIdProvided_ReturnItemWithId()
        {
            //Arrange

            //Act
            var result = Instance.TryFetchManyById(3);

            //Assert
            result.Should().BeEquivalentTo(new List<Result<Garbage>>
            {
                Result<Garbage>.Success(new Garbage
                {
                    Id = 3,
                    Name = "Terry",
                    Level = 18
                })
            });
        }

        [TestMethod]
        public void WhenMultipleIdsProvided_ReturnAllItemsWithIds()
        {
            //Arrange

            //Act
            var result = Instance.TryFetchManyById(1, 2, 4);

            //Assert
            result.Should().BeEquivalentTo(new List<Result<Garbage>>
            {
                Result<Garbage>.Success(new DerivedGarbage
                {
                    Id = 4,
                    Name = "Harry",
                    Level = 7,
                    Job = "That guy over there"
                }),
                Result<Garbage>.Success(new Garbage
                {
                    Id = 1,
                    Name = "Jerry",
                    Level = 20
                }),
                Result<Garbage>.Success(new DerivedGarbage
                {
                    Id = 2,
                    Name = "Garry",
                    Level = 45,
                    Job = "Some guy"
                })
            });
        }

        [TestMethod]
        public void WhenIdsNotFound_Throw()
        {
            //Arrange

            //Act
            var result = Instance.TryFetchManyById(-1, 99, 56);

            //Assert
            result.Should().BeEquivalentTo(new List<Result<Garbage>>
            {
                Result<Garbage>.Failure(), Result<Garbage>.Failure(), Result<Garbage>.Failure()
            });
        }
    }

    [TestClass]
    public class TryFetchManyById_Enumerable : Tester<GarbageReadOnlyBundleRepository>
    {
        [TestMethod]
        public void WhenNoIdProvided_Throw()
        {
            //Arrange
            IEnumerable<int> ids = null!;

            //Act
            var action = () => Instance.TryFetchManyById(ids);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("ids");
        }

        [TestMethod]
        public void WhenOnlyOneIdProvided_ReturnItemWithId()
        {
            //Arrange
            var ids = new List<int> { 3 };

            //Act
            var result = Instance.TryFetchManyById(ids);

            //Assert
            result.Should().BeEquivalentTo(new List<Result<Garbage>>
            {
                Result<Garbage>.Success(new Garbage
                {
                    Id = 3,
                    Name = "Terry",
                    Level = 18
                })
            });
        }

        [TestMethod]
        public void WhenMultipleIdsProvided_ReturnAllItemsWithIds()
        {
            //Arrange
            var ids = new List<int> { 1, 2, 4 };

            //Act
            var result = Instance.TryFetchManyById(ids);

            //Assert
            result.Should().BeEquivalentTo(new List<Result<Garbage>>
            {
                Result<Garbage>.Success(new DerivedGarbage
                {
                    Id = 4,
                    Name = "Harry",
                    Level = 7,
                    Job = "That guy over there"
                }),
                Result<Garbage>.Success(new Garbage
                {
                    Id = 1,
                    Name = "Jerry",
                    Level = 20
                }),
                Result<Garbage>.Success(new DerivedGarbage
                {
                    Id = 2,
                    Name = "Garry",
                    Level = 45,
                    Job = "Some guy"
                })
            });
        }

        [TestMethod]
        public void WhenIdsNotFound_Throw()
        {
            //Arrange
            var ids = new List<int> { -1, 99, 56 };

            //Act
            var result = Instance.TryFetchManyById(ids);

            //Assert
            result.Should().BeEquivalentTo(new List<Result<Garbage>>
            {
                Result<Garbage>.Failure(), Result<Garbage>.Failure(), Result<Garbage>.Failure()
            });
        }
    }

    [TestClass]
    public class TryFetchManyById_Params_SubEntity : Tester<GarbageReadOnlyBundleRepository>
    {
        [TestMethod]
        public void WhenNoIdProvided_ReturnEmpty()
        {
            //Arrange

            //Act
            var result = Instance.TryFetchManyById<DerivedGarbage>();

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenOnlyOneIdProvided_ReturnItemWithId()
        {
            //Arrange

            //Act
            var result = Instance.TryFetchManyById<DerivedGarbage>(2);

            //Assert
            result.Should().BeEquivalentTo(new List<Result<DerivedGarbage>>
            {
                Result<DerivedGarbage>.Success(new DerivedGarbage
                {
                    Id = 2,
                    Name = "Garry",
                    Level = 45,
                    Job = "Some guy"
                })
            });
        }

        [TestMethod]
        public void WhenMultipleIdsProvided_ReturnAllItemsWithIds()
        {
            //Arrange

            //Act
            var result = Instance.TryFetchManyById<DerivedGarbage>(2, 4);

            //Assert
            result.Should().BeEquivalentTo(new List<Result<DerivedGarbage>>
            {
                Result<DerivedGarbage>.Success(new DerivedGarbage
                {
                    Id = 4,
                    Name = "Harry",
                    Level = 7,
                    Job = "That guy over there"
                }),
                Result<DerivedGarbage>.Success(new DerivedGarbage
                {
                    Id = 2,
                    Name = "Garry",
                    Level = 45,
                    Job = "Some guy"
                })
            });
        }

        [TestMethod]
        public void WhenIdsNotFound_Throw()
        {
            //Arrange

            //Act
            var result = Instance.TryFetchManyById<DerivedGarbage>(1, 3, 56);

            //Assert
            result.Should().BeEquivalentTo(new List<Result<DerivedGarbage>>
            {
                Result<DerivedGarbage>.Failure(), Result<DerivedGarbage>.Failure(), Result<DerivedGarbage>.Failure()
            });
        }
    }

    [TestClass]
    public class TryFetchManyById_Enumerable_SubEntity : Tester<GarbageReadOnlyBundleRepository>
    {
        [TestMethod]
        public void WhenNoIdProvided_Throw()
        {
            //Arrange
            IEnumerable<int> ids = null!;

            //Act
            var action = () => Instance.TryFetchManyById<DerivedGarbage>(ids);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("ids");
        }

        [TestMethod]
        public void WhenOnlyOneIdProvided_ReturnItemWithId()
        {
            //Arrange
            var ids = new List<int> { 2 };

            //Act
            var result = Instance.TryFetchManyById<DerivedGarbage>(ids);

            //Assert
            result.Should().BeEquivalentTo(new List<Result<DerivedGarbage>>
            {
                Result<DerivedGarbage>.Success(new DerivedGarbage
                {
                    Id = 2,
                    Name = "Garry",
                    Level = 45,
                    Job = "Some guy"
                })
            });
        }

        [TestMethod]
        public void WhenMultipleIdsProvided_ReturnAllItemsWithIds()
        {
            //Arrange
            var ids = new List<int> { 2, 4 };

            //Act
            var result = Instance.TryFetchManyById<DerivedGarbage>(ids);

            //Assert
            result.Should().BeEquivalentTo(new List<Result<DerivedGarbage>>
            {
                Result<DerivedGarbage>.Success(new DerivedGarbage
                {
                    Id = 4,
                    Name = "Harry",
                    Level = 7,
                    Job = "That guy over there"
                }),
                Result<DerivedGarbage>.Success(new DerivedGarbage
                {
                    Id = 2,
                    Name = "Garry",
                    Level = 45,
                    Job = "Some guy"
                })
            });
        }

        [TestMethod]
        public void WhenIdsNotFound_Throw()
        {
            //Arrange
            var ids = new List<int> { 1, 3, 56 };

            //Act
            var result = Instance.TryFetchManyById<DerivedGarbage>(ids);

            //Assert
            result.Should().BeEquivalentTo(new List<Result<DerivedGarbage>>
            {
                Result<DerivedGarbage>.Failure(), Result<DerivedGarbage>.Failure(), Result<DerivedGarbage>.Failure()
            });
        }
    }
}