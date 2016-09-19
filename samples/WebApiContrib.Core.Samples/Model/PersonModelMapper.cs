using WebApiContrib.Core.Versioning;

namespace WebApiContrib.Core.Samples.Model
{
    public class PersonModelMapper : IModelMapper<PersonModel>
    {
        public object Map(PersonModel model, int? version)
        {
            if (version == 2)
            {
                return new PersonModel.V2(model);
            }

            return model;
        }
    }
}