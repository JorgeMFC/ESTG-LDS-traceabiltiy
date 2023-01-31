import * as yup from "yup";

export const addModelFormSchema = (
  colorsId: string[],
  sizesId: string[],
  productionProcessesId: string[]
) => {
  return yup.object().shape({
    id: yup.string().min(3, "Name must be at least 3 characters long").required("Required"),
    name: yup.string().min(3, "Name must be at least 3 characters long").required("Required"),
    colors: yup.array().of(
      yup.object().shape({
        componentId: yup.string().oneOf(colorsId, "Invalid Color"),
      })
    ),
    sizes: yup.array().of(
      yup.object().shape({
        componentId: yup.string().oneOf(sizesId, "Invalid Size"),
      })
    ),
    productionProcess: yup.string().oneOf(productionProcessesId, "Invalid Production Process"),
  });
};
