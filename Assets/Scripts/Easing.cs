/*
 * Adapted from jQuery Easing v1.3 - http://gsgd.co.uk/sandbox/jquery/easing/
 *
 * Uses the built in easing capabilities added In jQuery 1.1
 * to offer multiple easing options
 *
 * TERMS OF USE - jQuery Easing
 * 
 * Open source under the BSD License. 
 * 
 * Copyright © 2008 George McGinley Smith
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, 
 * are permitted provided that the following conditions are met:
 * 
 * Redistributions of source code must retain the above copyright notice, this list of 
 * conditions and the following disclaimer.
 * Redistributions in binary form must reproduce the above copyright notice, this list 
 * of conditions and the following disclaimer in the documentation and/or other materials 
 * provided with the distribution.
 * 
 * Neither the name of the author nor the names of contributors may be used to endorse 
 * or promote products derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF
 * MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE
 *  COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
 *  EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE
 *  GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED 
 * AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 *  NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED 
 * OF THE POSSIBILITY OF SUCH DAMAGE. 
 *
*/
using UnityEngine;

public static class Easing
{
    /// <summary>
    /// Ease In Quad
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns>Eased value</returns>
    public static float EaseInQuad(float t, float b = 0, float c = 1f, float d = 1f)
    {
        return c * (t /= d) * t + b;
    }

    /// <summary>
    /// Ease Out Quad
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns>Eased value</returns>
    public static float EaseOutQuad(float t, float b = 0, float c = 1f, float d = 1f)
    {
        return -c * (t /= d) * (t - 2) + b;
    }

    /// <summary>
    /// Ease In Out Quad
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns>Eased value</returns>
    public static float EaseInOutQuad(float t, float b = 0, float c = 1f, float d = 1f)
    {
        if ((t /= d / 2) < 1)
        {
            return c / 2 * t * t + b;
        }

        return -c / 2 * ((--t) * (t - 2) - 1) + b;
    }

    /// <summary>
    /// Ease In Cubic
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns>Eased value</returns>
    public static float EaseInCubic(float t, float b = 0, float c = 1f, float d = 1f)
    {
        return c * (t /= d) * t * t + b;
    }

    /// <summary>
    /// Ease Out Cubic
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns>Eased value</returns>
    public static float EaseOutCubic(float t, float b = 0, float c = 1f, float d = 1f)
    {
        return c * ((t = t / d - 1) * t * t + 1) + b;
    }

    /// <summary>
    /// Ease In Out Cubic
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns>Eased value</returns>
    public static float EaseInOutCubic(float t, float b = 0, float c = 1f, float d = 1f)
    {
        if ((t /= d / 2) < 1)
        {
            return c / 2 * t * t * t + b;
        }

        return c / 2 * ((t -= 2) * t * t + 2) + b;
    }

    /// <summary>
    /// Ease In Quart
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns>Eased value</returns>
    public static float EaseInQuart(float t, float b = 0, float c = 1f, float d = 1f)
    {
        return c * (t /= d) * t * t * t + b;
    }

    /// <summary>
    /// Ease Out Quart
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns>Eased value</returns>
    public static float EaseOutQuart(float t, float b = 0, float c = 1f, float d = 1f)
    {
        return -c * ((t = t / d - 1) * t * t * t - 1) + b;
    }

    /// <summary>
    /// Ease In Out Quart
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns>Eased value</returns>
    public static float EaseInOutQuart(float t, float b = 0, float c = 1f, float d = 1f)
    {
        if ((t /= d / 2) < 1)
        {
            return c / 2 * t * t * t * t + b;
        }

        return -c / 2 * ((t -= 2) * t * t * t - 2) + b;
    }
    /// <summary>
    /// Ease In Quint
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns>Eased value</returns>
    public static float EaseInQuint(float t, float b = 0, float c = 1f, float d = 1f)
    {
        return c * (t /= d) * t * t * t * t + b;
    }

    /// <summary>
    /// Ease Out Quint
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns>Eased value</returns>
    public static float EaseOutQuint(float t, float b = 0, float c = 1f, float d = 1f)
    {
        return c * ((t = t / d - 1) * t * t * t * t + 1) + b;
    }

    /// <summary>
    /// Ease In Out Quint
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns>Eased value</returns>
    public static float EaseInOutQuint(float t, float b = 0, float c = 1f, float d = 1f)
    {
        if ((t /= d / 2) < 1)
        {
            return c / 2 * t * t * t * t * t + b;
        }

        return c / 2 * ((t -= 2) * t * t * t * t + 2) + b;
    }

    /// <summary>
    /// Ease In Sine
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns>Eased value</returns>
    public static float EaseInSine(float t, float b = 0, float c = 1f, float d = 1f)
    {
        return -c * Mathf.Cos(t / d * (Mathf.PI / 2)) + c + b;
    }

    /// <summary>
    /// Ease Out Sin
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns>Eased value</returns>
    public static float EaseOutSine(float t, float b = 0, float c = 1f, float d = 1f)
    {
        return c * Mathf.Sin(t / d * (Mathf.PI / 2)) + b;
    }

    /// <summary>
    /// Ease In Out Sin
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns>Eased value</returns>
    public static float EaseInOutSine(float t, float b = 0, float c = 1f, float d = 1f)
    {
        return -c / 2 * (Mathf.Cos(Mathf.PI * t / d) - 1) + b;
    }

    /// <summary>
    /// Ease In Expo
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns>Eased value</returns>
    public static float EaseInExpo(float t, float b = 0, float c = 1f, float d = 1f)
    {
        return (t == 0) ? b : c * Mathf.Pow(2, 10 * (t / d - 1)) + b;
    }

    /// <summary>
    /// Ease Out Expo
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns>Eased value</returns>
    public static float EaseOutExpo(float t, float b = 0, float c = 1f, float d = 1f)
    {
        return (t == d) ? b + c : c * (-Mathf.Pow(2, -10 * t / d) + 1) + b;
    }

    /// <summary>
    /// Ease In Out Expo
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns>Eased value</returns>
    public static float EaseInOutExpo(float t, float b = 0, float c = 1f, float d = 1f)
    {
        if (t == 0)
        {
            return b;
        }

        if (t == d)
        {
            return b + c;
        }

        if ((t /= d / 2) < 1)
        {
            return c / 2 * Mathf.Pow(2, 10 * (t - 1)) + b;
        }

        return c / 2 * (-Mathf.Pow(2, -10 * --t) + 2) + b;
    }

    /// <summary>
    /// Ease In Circ
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns>Eased value</returns>
    public static float EaseInCirc(float t, float b = 0, float c = 1f, float d = 1f)
    {
        return -c * (Mathf.Sqrt(1 - (t /= d) * t) - 1) + b;
    }

    /// <summary>
    /// Ease Out Circ
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns>Eased value</returns>
    public static float EaseOutCirc(float t, float b = 0, float c = 1f, float d = 1f)
    {
        return c * Mathf.Sqrt(1 - (t = t / d - 1) * t) + b;
    }

    /// <summary>
    /// Ease In Out Circ
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns>Eased value</returns>
    public static float EaseInOutCirc(float t, float b = 0, float c = 1f, float d = 1f)
    {
        if ((t /= d / 2) < 1)
        {
            return -c / 2 * (Mathf.Sqrt(1 - t * t) - 1) + b;
        }

        return c / 2 * (Mathf.Sqrt(1 - (t -= 2) * t) + 1) + b;
    }

    /// <summary>
    /// Ease In Elastic
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns>Eased value</returns>
    public static float EaseInElastic(float t, float b = 0, float c = 1f, float d = 1f)
    {
        var s = 1.70158f;
        var p = 0f;
        var a = c;

        if (t == 0)
        {
            return b;
        }

        if ((t /= d) == 1)
        {
            return b + c;
        }

        if (p == 0)
        {
            p = d * .3f;
        }

        if (a < Mathf.Abs(c))
        {
            a = c;
            s = p / 4;
        }
        else
        {
            s = p / (2 * Mathf.PI) * Mathf.Asin(c / a);
        }

        return -(a * Mathf.Pow(2, 10 * (t -= 1)) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p)) + b;
    }

    /// <summary>
    /// Ease Out Elastic
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns>Eased value</returns>
    public static float EaseOutElastic(float t, float b = 0, float c = 1f, float d = 1f)
    {
        var s = 1.70158f;
        var p = 0f;
        var a = c;

        if (t == 0)
        {
            return b;
        }

        if ((t /= d) == 1)
        {
            return b + c;
        }

        if (p == 0)
        {
            p = d * .3f;
        }

        if (a < Mathf.Abs(c))
        {
            a = c;
            s = p / 4;
        }
        else
        {
            s = p / (2 * Mathf.PI) * Mathf.Asin(c / a);
        }

        return a * Mathf.Pow(2, -10 * t) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p) + c + b;
    }

    /// <summary>
    /// Ease In Out Elastic
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns>Eased value</returns>
    public static float EaseInOutElastic(float t, float b = 0, float c = 1f, float d = 1f)
    {
        var s = 1.70158f;
        var p = 0f;
        var a = c;

        if (t == 0)
        {
            return b;
        }

        if ((t /= d / 2) == 2)
        {
            return b + c;
        }

        if (p != 0)
        {
            p = d * (.3f * 1.5f);
        }

        if (a < Mathf.Abs(c))
        {
            a = c;
            s = p / 4;
        }
        else
        {
            s = p / (2 * Mathf.PI) * Mathf.Asin(c / a);
        }

        if (t < 1)
        {
            return -.5f * (a * Mathf.Pow(2, 10 * (t -= 1)) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p)) + b;
        }

        return a * Mathf.Pow(2, -10 * (t -= 1)) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p) * .5f + c + b;
    }

    /// <summary>
    /// Ease In Back
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns>Eased value</returns>
    public static float EaseInBack(float t, float b = 0, float c = 1f, float d = 1f, float? s = null)
    {
        var s2 = s ?? 1.70158f;

        return c * (t /= d) * t * ((s2 + 1) * t - s2) + b;
    }

    /// <summary>
    /// Ease Out Back
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns>Eased value</returns>
    public static float EaseOutBack(float t, float b = 0, float c = 1f, float d = 1f, float? s = null)
    {
        var s2 = s ?? 1.70158f;

        return c * ((t = t / d - 1) * t * ((s2 + 1) * t + s2) + 1) + b;
    }

    /// <summary>
    /// Ease In Out Back
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns>Eased value</returns>
    public static float EaseInOutBack(float t, float b = 0, float c = 1f, float d = 1f, float? s = null)
    {
        var s2 = s ?? 1.70158f;

        if ((t /= d / 2) < 1)
        {
            return c / 2 * (t * t * (((s2 *= (1.525f)) + 1) * t - s2)) + b;
        }

        return c / 2 * ((t -= 2) * t * (((s2 *= (1.525f)) + 1) * t + s2) + 2) + b;
    }

    /// <summary>
    /// Ease In Bounce
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns>Eased value</returns>
    public static float EaseInBounce(float t, float b = 0, float c = 1f, float d = 1f)
    {
        return c - EaseOutBounce(d - t, 0, c, d) + b;
    }

    /// <summary>
    /// Ease Out Bounce
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns>Eased value</returns>
    public static float EaseOutBounce(float t, float b = 0, float c = 1f, float d = 1f)
    {
        if ((t /= d) < (1 / 2.75))
        {
            return c * (7.5625f * t * t) + b;
        }
        else if (t < (2 / 2.75))
        {
            return c * (7.5625f * (t -= (1.5f / 2.75f)) * t + .75f) + b;
        }
        else if (t < (2.5 / 2.75))
        {
            return c * (7.5625f * (t -= (2.25f / 2.75f)) * t + .9375f) + b;
        }
        else
        {
            return c * (7.5625f * (t -= (2.625f / 2.75f)) * t + .984375f) + b;
        }
    }

    /// <summary>
    /// Ease In Out Bounce
    /// </summary>
    /// <param name="t">Current time</param>
    /// <param name="b">Start value</param>
    /// <param name="c">Change in value</param>
    /// <param name="d">Duration</param>
    /// <returns>Eased value</returns>
    public static float EaseInOutBounce(float t, float b = 0, float c = 1f, float d = 1f)
    {
        if (t < d / 2)
        {
            return EaseInBounce(t * 2, 0, c, d) * .5f + b;
        }

        return EaseOutBounce(t * 2 - d, 0, c, d) * .5f + c * .5f + b;
    }
}
