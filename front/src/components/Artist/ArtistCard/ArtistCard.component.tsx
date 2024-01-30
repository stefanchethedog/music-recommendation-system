import { FC } from "react";
import classNames from "classnames";
import Delete from "@mui/icons-material/Delete";
import axios from "axios";

import { DELETE_ARTIST } from "../../../endpoints";

import "./ArtistCard.styles.scss";
import useSnackbar from "../../../Hooks/useSnackbar";

export type ArtistProps = {
  id: String;
  name: String;
  className?: String;
};

const ArtistCard: FC<ArtistProps> = ({ className: classes, name, id }) => {
  const className = classNames("artist", classes);

  const { createSnackbar } = useSnackbar({
    message: "Artist deleted.",
    errorMessage: "Error, couldn't delete artist",
  });

  const handleDeleteArtist = async () => {
    axios
      .delete(DELETE_ARTIST(id))
      .then(() => {
        createSnackbar({ error: false });
      })
      .catch(() => {
        createSnackbar({ error: true });
      });
  };
  return (
    <div className={className}>
      <Delete className="artist__delete" onClick={handleDeleteArtist} />
      <div className="artist__name">{name}</div>
    </div>
  );
};

export default ArtistCard;
