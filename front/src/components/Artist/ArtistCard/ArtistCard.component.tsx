import { FC } from "react";
import classNames from "classnames";

import "./ArtistCard.styles.scss";

export type ArtistProps = {
  name: String;
  className?: String;
};

const ArtistCard: FC<ArtistProps> = ({ className: classes, name }) => {
  const className = classNames("artist", classes);
  return (
    <div className={className}>
      <div className="artist__name">{name}</div>
    </div>
  );
};

export default ArtistCard;
