import * as yup from "yup"

export const addComponentTypeFormSchema = () => {
	return yup.object().shape({
		componentType: yup
			.string()
			.min(3, "Component Type must be at least 3 characters long")
			.required("Required"),
	})
}

export const addStageFormSchema = () => {
	return yup.object().shape({
		id: yup
			.string()
			.min(3, "Id must be at least 3 characters long")
			.max(10, "Id must not exceed 10 characters long")
			.required("Required"),
		stage: yup.string().min(3, "Stage must be at least 3 characters long").required("Required"),
	})
}
