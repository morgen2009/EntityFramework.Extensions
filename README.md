# EntityFramework.Extensions

Entity Framework 6 does not support automatic code-fist migrations for views, triggers, as well doesn't have other nice features like
check constraints, permissions, because of some whom opinion that this information doesn't belong to the data model. This is not so at
least for enterprise applications. I like EF, but missing such features makes me difficult synchronization of C# source code and
database, or arrangement of automatic software deployment to the customers systems.

Vision

* automatic migrations for triggers
* automatic migrations for views
* check constraints for enums
* update column/table description from source code comments


Currently his is completely unstable version without any guarantee. Reading MSDN and source code of EF on github I changed my mind many
times, because unfortunately MS has made many extensions points as private or internal.
