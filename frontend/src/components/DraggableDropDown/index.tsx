import { useSortable } from "@dnd-kit/sortable"
import { CSS } from "@dnd-kit/utilities"
import { MdOutlineDragIndicator } from "react-icons/md"
import MySelect from "../controls/MySelect"

export function DraggableDropDown(props: any) {
	const { attributes, listeners, setNodeRef, transform, transition, isDragging } = useSortable({
		id: props.id,
	})

	const style = {
		transition,
		transform: CSS.Transform.toString(transform),
		opacity: isDragging ? 0.5 : 1,
	}

	return (
		<div
			ref={setNodeRef}
			style={style}
			{...attributes}
			{...listeners}
			className="flex items-center mb-2"
		>
			<div className="flex w-1/12 justify-center">
				<MdOutlineDragIndicator fontSize={20} />
			</div>
			<div className="w-11/12">
				<MySelect label={props.selectLabel} name={props.selectName}>
					<option disabled value="">
						Please select a stage
					</option>
					{props.selectOptions.map((option: any) => (
						<option key={option.id} value={option.id}>
							{option.stageName}
						</option>
					))}
				</MySelect>
			</div>
		</div>
	)
}
