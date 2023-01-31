import { FaTrash } from "react-icons/fa";
import { Color } from "../../../../models/api/Color";
import { Size } from "../../../../models/api/Size";
import NoImage from "../../../../assets/noimage.jpg";

interface ModelCardProps {
  name: string;
  colors: Array<Color>;
  sizes: Array<Size>;
  image: string | null;
  openEditCard: () => void;
  openRemoveCard: () => void;
}

export const ModelCard = (props: ModelCardProps) => {
  let colorsList: Array<string> = [];
  let sizesList: Array<string> = [];

  props.colors.map((color, index) => {
    props.colors.length - 1 == index
      ? colorsList.push(color.color1)
      : colorsList.push(`${color.color1}, `);
  });

  props.sizes.map((size, index) => {
    props.sizes.length - 1 == index
      ? sizesList.push(size.size1)
      : sizesList.push(`${size.size1}, `);
  });

  return (
    <div className="flex flex-col bg-white rounded-2xl shadow-md ">
      <div className="h-4/5 cursor-pointer" onClick={props.openEditCard}>
        <img
          src={props.image ? `${import.meta.env.VITE_BASE_URL_API}${props.image}` : NoImage}
          className="object-cover h-full w-full h-36 rounded-t-2xl"
          alt=""
        />
      </div>

      <h1 className="font-bold text-center text-gray-500">{props.name}</h1>
      <div className="flex items-center px-2 pb-2 pt-1 gap-1">
        <div style={{ width: "85%" }}>
          <h1 className="font-semibold text-gray-500 text-sm truncate ...">Colors: {colorsList}</h1>
          <h1 className="font-semibold text-gray-500 text-sm truncate ...">Sizes: {sizesList}</h1>
        </div>
        <div>
          <button
            onClick={props.openRemoveCard}
            className="text-white bg-gray-500 border border-gray-500 hover:bg-gray-700 focus:outline-none font-medium rounded-lg text-sm px-1.5 py-2 text-center transition"
          >
            <FaTrash />
          </button>
        </div>
      </div>
    </div>
  );
};
