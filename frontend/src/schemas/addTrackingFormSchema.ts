import * as yup from 'yup';

export const addTrackingFormSchema = (
  modelsId: string[],
  modelsColorsId: string[],
  modelsSizesId: string[],
  clientsId: string[],
  clientsAddressesId: string[],
  stagesModelsId: string[]
) => {
  return yup.object().shape({
    id: yup
      .string()
      .min(3, 'id must be at least 3 characters long')
      .required('Required'),
    modelId: yup.string().required('Required'),
    modelColorId: yup.string().required('Required'),
    modelSizeId: yup.string().required('Required'),
    clientId: yup.string().required('Required'),
    clientAddressId: yup.string().required('Required'),
    stagesModelId: yup.string().required('Required'),
    amount: yup.number().min(1).required(),
  });
};
