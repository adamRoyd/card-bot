using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SNGEGT
{
    public class IcmIntegrator
    {
        Game game;
        public IcmIntegrator()
        {
            game = new Game();
        }

        public double GetExpectedValue()
        {
            return 0;
        }


        private void SetRanges()
        {
            BlindInfo blinds = new BlindInfo(100,200,0);

            game.ICMRanges(blinds, false, (Award)comboBoxGame.SelectedItem, radioButtonPush.Checked);
        }

    }
}
