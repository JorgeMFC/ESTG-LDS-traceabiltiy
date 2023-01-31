import { Form, Formik, FormikProps } from "formik"

import { Button } from "flowbite-react/lib/esm/components/Button"
import { useRef } from "react"
import toast from "react-hot-toast"
import { ThreeDots } from "react-loader-spinner"
import { useMutation, useQuery, useQueryClient } from "react-query"
import MyInput from "../../../../../components/controls/MyInput"
import { ComponentTypePaginated } from "../../../../../models/api/ComponentType"
import { addComponentTypeFormSchema } from "../../../../../schemas/organizationFormSchema"
import {
	addComponentType,
	deleteComponentType,
	getComponentTypes,
} from "../../../../../services/api/componentsTypeApi"
import "../style.scss"
import { Badge } from "./Badge"

interface ComponentTypeFormValues {
	componentType: string
}

export const ComponentTypesSection = () => {
	const queryClient = useQueryClient()

	const formRef = useRef<FormikProps<ComponentTypeFormValues>>(null)

	const {
		isLoading: isLoadingComponentTypes,
		isError: isErrorComponentTypes,
		error: errorComponentTypes,
		data: componentTypes,
	} = useQuery<ComponentTypePaginated, Error>("componentTypes-page-organization", () =>
		getComponentTypes(1, 1000)
	)

	const addComponentTypeMutation = useMutation(addComponentType, {
		onSuccess: (response) => {
			// Invalidates cache and refetch
			queryClient.invalidateQueries("componentTypes-page-organization")
			toast.success("Component type inserted successfully!")
		},
		onError: (error: Error) => {
			toast.error(error.message)
		},
	})

	const deleteComponentTypeMutation = useMutation(deleteComponentType, {
		onSuccess: (response) => {
			// Invalidates cache and refetch
			queryClient.invalidateQueries("componentTypes-page-organization")
			toast.success("Component Type deleted successfully!")
		},
		onError: (error: Error) => {
			toast.error(error.message)
		},
	})

	const onSubmit = (values: ComponentTypeFormValues, actions: any) => {
		console.log(values)
		actions.resetForm()
		addComponentTypeMutation.mutate({
			id: -1,
			organizationId: 1,
			componentType: values.componentType,
		})
	}

	const handleSubmit = () => {
		if (formRef.current) {
			formRef.current.handleSubmit()
		}
	}

	let content: any

	if (isLoadingComponentTypes) {
		content = <ThreeDots color="#3b82f6" visible={true} />
	} else if (isErrorComponentTypes) {
		content = <h1 className="font-semibold text-red-600">{errorComponentTypes.message}</h1>
	} else {
		content = componentTypes?.results.map((element) => {
			return (
				<Badge
					key={element.componentsType.id}
					onRemove={() => {
						element.componentsType.id != null
							? deleteComponentTypeMutation.mutate({
									id: element.componentsType.id,
							  })
							: toast.error("Invalid id")
					}}
					badgeContent={{
						id: element.componentsType.id || -1,
						name: element.componentsType.componentType,
					}}
				/>
			)
		})
	}

	return (
		<div className="flex flex-col gap-6 mt-20">
			<h1 className="text-xl font-bold text-gray-600">Component Types</h1>
			<Formik
				innerRef={formRef}
				initialValues={{ componentType: "" }}
				validationSchema={addComponentTypeFormSchema}
				onSubmit={onSubmit}
			>
				<Form>
					<div className="flex items-center gap-6">
						<div className="w-2/5">
							<MyInput
								label="Component Type"
								name="componentType"
								type="text"
								placeholder="Component Type"
							/>
						</div>
						<div>
							<Button color="dark" size="md" onClick={handleSubmit}>
								Add Component Type
							</Button>
						</div>
					</div>
				</Form>
			</Formik>
			<div className="flex flex-wrap gap-4 max-h-56 overflow-y-auto">{content}</div>
		</div>
	)
}
