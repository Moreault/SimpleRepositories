![SimpleRepositories](https://github.com/Moreault/SimpleRepositories/blob/master/simplerepositories.png)
# SimpleRepositories
Base classes and interfaces for setting up simple repositories.

## Bundles

### What the hell is a bundle?
A bundle is basically just a *bundle* of entities which is typically stored in a serializable file that you can query and/or save to.

It's useful if you need to store data but don't necessarily require a database or if you're just prototyping a quick and dirty idea. 

For your information, I use ReadOnlyBundleRepository and BundleRepository to store Rough Trigger's game data such as spritesheet maps (holds sprite positions and sizes,) tilesets or items.

### Does it have to be a serialized file?
No. It can be just about anything, really. One of the only contraint for bundles is that entities must have an Id property. This id is automatically incremented whenever a new entity is added to the bundle using ToolBX.Unicity.

### Getting started
Check out the unit test project for samples on how to use it.

### Upcoming changes
2.1.0 (currently in beta) will add an abstract StartingId property to the BundleRepository class for cases where you might want something other than zero as a starting ID in your repository.

The name and accessiblity of this property might change from now to official release. It could end up being virtual since wanting a different starting ID is kind of an exceptional case and not necessarily standard practice but I don't like having virtual properties on base classes because then you kind of "have to guess" its existence. Forcing it to be set leaves nothing to guesses. We'll see how it turns out and adjust accordingly.