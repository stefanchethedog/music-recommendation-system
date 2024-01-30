import { FC } from "react";
import classNames from "classnames";
import Delete from "@mui/icons-material/Delete";
import "./GenreCard.styles.scss";
import useSnackbar from "../../../Hooks/useSnackbar";
import axios from "axios";
import { DELETE_GENRE } from "../../../endpoints";

export type GenreProps = {
  id: String;
  name: String;
  className?: String;
};

const GenreCard: FC<GenreProps> = ({ className: classes, name, id }) => {
  const className = classNames("genre", classes);
  const { createSnackbar } = useSnackbar({
    message: "Genre deleted.",
    errorMessage: "Error, couldn't delete genre",
  });

  const handleDeleteGenre = async () => {
    axios
      .delete(DELETE_GENRE(id))
      .then(() => {
        createSnackbar({ error: false });
      })
      .catch(() => {
        createSnackbar({ error: true });
      });
  };
  return (
    <div className={className}>
      <Delete className="genre__delete" onClick={handleDeleteGenre}/>
      <div className="genre__name">{name}</div>
    </div>
  );
};

export default GenreCard;
