import * as yup from "yup"

export const addClientFormSchema = (stagesId: string[]) => {
	return yup.object().shape({
		id: yup.string().min(3, "Id must be at least 3 characters long").required("Required"),
		name: yup.string().min(3, "Name must be at least 3 characters long").required("Required"),
		email: yup
			.string()
			.min(3, "email must be at least 3 characters long")
			.email()
			.required("Required"),
		addresses: yup.array().of(
			yup.object().shape({
				street: yup
					.string()
					.min(3, "Street must be at least 3 characters long")
					.required("Required"),
				postalCode: yup
					.string()
					.matches(RegExp("^\\d{4}-\\d{3}$"), "The postal code is not valid")
					.required("Required"),
			})
		),
	})
}
