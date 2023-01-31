import { Form, Formik, FormikProps } from "formik"

import { Button } from "flowbite-react/lib/esm/components/Button"
import { useRef } from "react"
import toast from "react-hot-toast"
import { ThreeDots } from "react-loader-spinner"
import { useMutation, useQuery, useQueryClient } from "react-query"
import MyInput from "../../../../../components/controls/MyInput"
import { StagePaginated } from "../../../../../models/api/Stage"
import { addStageFormSchema } from "../../../../../schemas/organizationFormSchema"
import { addStage, deleteStage, getStages } from "../../../../../services/api/stageApi"
import "../style.scss"
import { Badge } from "./Badge"

interface StageFormValues {
	id: string
	stage: string
}

export const StagesSection = () => {
	const queryClient = useQueryClient()

	const formRef = useRef<FormikProps<StageFormValues>>(null)

	const {
		isLoading: isLoadingStages,
		isError: isErrorStages,
		error: errorStages,
		data: stages,
	} = useQuery<StagePaginated, Error>("stages-page-organization", getStages)

	const addStageMutation = useMutation(addStage, {
		onSuccess: (response) => {
			// Invalidates cache and refetch
			queryClient.invalidateQueries("stages-page-organization")
			toast.success("Stage inserted successfully!")
		},
		onError: (error: Error) => {
			toast.error(error.message)
		},
	})

	const deleteStageMutation = useMutation(deleteStage, {
		onSuccess: (response) => {
			// Invalidates cache and refetch
			queryClient.invalidateQueries("stages-page-organization")
			toast.success("Stage deleted successfully!")
		},
		onError: (error: Error) => {
			toast.error(error.message)
		},
	})

	const onSubmit = (values: StageFormValues, actions: any) => {
		console.log(values)
		actions.resetForm()
		addStageMutation.mutate({
			stage: {
				id: values.id,
				organizationId: 1,
				stageName: values.stage,
				stageDescription: "Description",
				stagesTypeId: "ST02",
			},
		})
	}

	const handleSubmit = () => {
		if (formRef.current) {
			formRef.current.handleSubmit()
		}
	}

	let content: any

	if (isLoadingStages) {
		content = <ThreeDots color="#3b82f6" visible={true} />
	} else if (isErrorStages) {
		content = <h1 className="font-semibold text-red-600">{errorStages.message}</h1>
	} else {
		content = stages?.results.map((element) => {
			return (
				<Badge
					key={element.id}
					onRemove={() => {
						element.id != null
							? deleteStageMutation.mutate({
									id: element.id,
							  })
							: toast.error("Invalid id")
					}}
					badgeContent={{
						id: element.id || "",
						name: element.stageName,
					}}
				/>
			)
		})
	}

	return (
		<div className="flex flex-col gap-6 mt-20">
			<h1 className="text-xl font-bold text-gray-600">Stages</h1>
			<Formik
				innerRef={formRef}
				initialValues={{ stage: "", id: "" }}
				validationSchema={addStageFormSchema}
				onSubmit={onSubmit}
			>
				<Form>
					<div className="flex items-center gap-6">
						<MyInput label="StageId" name="id" type="text" placeholder="StageId" />
						<MyInput label="Stage" name="stage" type="text" placeholder="Stage" />
						<div>
							<Button color="dark" size="md" onClick={handleSubmit}>
								Add Stage
							</Button>
						</div>
					</div>
				</Form>
			</Formik>
			<div className="flex flex-wrap gap-4 max-h-56 overflow-y-auto">{content}</div>
		</div>
	)
}
