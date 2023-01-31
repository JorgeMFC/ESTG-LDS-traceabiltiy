import * as yup from "yup"

export const addComponentTypeFormSchema = () => {
	return yup.object().shape({
		componentType: yup
			.string()
			.min(3, "Component Type must be at least 3 characters long")
			.required("Required"),
	})
}
