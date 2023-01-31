import * as yup from "yup";

export const addProviderFormSchema = (componentsId: string[]) => {
  return yup.object().shape({
    providerId: yup.string().min(3, "Id must be at least 3 characters long").required("Required"),
    providerName: yup
      .string()
      .min(3, "Name must be at least 3 characters long")
      .required("Required"),
    providerEmail: yup.string().email().required("Required"),
    components: yup.array().of(
      yup.object().shape({
        componentId: yup.string().oneOf(componentsId, "Invalid Component"),
      })
    ),
  });
};
