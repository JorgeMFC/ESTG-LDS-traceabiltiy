import * as yup from "yup";

export const registrationFormSchema = () => {
  return yup.object().shape({
    name: yup.string().required("Required"),
    email: yup.string().email().required("Required"),
    birthDate: yup
      .date()
      .min("1900-01-01", "Invalid Birth date")
      .nullable()
      .default(null)
      .test("Old Enough", "Age is under 16", (value) =>
        value != null ? new Date().getFullYear() - value.getFullYear() >= 16 : true
      ),
  });
};
